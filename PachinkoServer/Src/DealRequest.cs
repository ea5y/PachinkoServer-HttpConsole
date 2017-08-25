using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LitJson;
using XY.Pachinko.CommunicationData;
using System.Text.RegularExpressions;
using YduCs;

namespace PachinkoServer
{
    public class DealRequest
    {
        private static Config _config;
        private static Dictionary<string, DealBase> _dealDic;
        

        private static void SetDealDic(Config config)
        {
            _dealDic = new Dictionary<string, DealBase>() {
                {URL.PATH_YDU, new DealYdu(config)},
                {URL.PATH_SWITCH, new DealSwitch(config)},
                {URL.PATH_CHANGE_LEVEL, new DealLevel(config) },
                {URL.PATH_YDU_GET_INPUT, new DealInput(config) }
            };    
        }

        public static string Run(HttpListenerContext context)
        {
            //get pre path
            var prePath = GetPrePath(context.Request.Url.ToString());
            //Create Deal
            if(_dealDic.ContainsKey(prePath))
            {
                DealBase deal = _dealDic[prePath];
                //get result
                var result = deal.GetResult(context, prePath);
                return result;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        private static string GetPrePath(string url)
        {
            var match = Regex.Match(url, @"[a-zA-z]+://[^\/]*").ToString();
            var index = url.IndexOf('?');
            var sub = url.Substring(0, index);
            sub = sub.Substring(match.Length, sub.Length - match.Length);
            Console.WriteLine("Prepath: {0}", sub);
            return sub;
        }

        public static void InitConfig(Config config)
        {
            _config = config;//IOHelper.ReadFromJson<Config>("./");
            SetDealDic(_config);
        }
    }

    public abstract class DealBase
    {
        protected Config config;

        public DealBase(Config config)
        {
            this.config = config;
        }

        public abstract string GetResult(HttpListenerContext context, string Path);

        protected void YduOpen(ushort unitId, string modelName)
        {

        }

        protected void YduClose(ushort unitId)
        {

        }

        protected void YduOutput(ushort unitId, byte[] outputData, ushort start, ushort count)
        {

        }

        protected string YduInput(ushort unitId, byte[] inputData, ushort start, ushort count)
        {
            string msg = string.Empty;
            int result = YduDio.Input(unitId, inputData, start, count);
            if (result == Ydu.YDU_RESULT_SUCCESS)
            {
                var str = "Yduinput success";
                Console.WriteLine(str);
                msg = str;
            }
            else
            {
                var str = string.Format("Yduinput error: 0x{0:X}", result);
                Console.WriteLine(str);
                msg = str;
            }

            return msg;
        }
    }

    public class DealYdu : DealBase
    {
        public DealYdu(Config config):base(config)
        {
        }

        public override string GetResult(HttpListenerContext context, string Path)
        {
            var data = context.Request.QueryString["data"];
            var type = context.Request.QueryString["type"];
            var res = new DIORes();

            var unitId = context.Request.QueryString["unitid"];
            var modelName = context.Request.QueryString["modelname"];

            if(type == URL.PARAM_YDU_TYPE_OPEN)
            {
                res.Result = this.Open((ushort)Convert.ToInt16(unitId), modelName);
            }
            else if(type == URL.PARAM_YDU_TYPE_CLOSE)
            {
                res.Result = this.Close((ushort)Convert.ToInt16(unitId));
            }

            return JsonMapper.ToJson(res);
        }

        private string Open(ushort unitId, string modelName)
        {
            string msg = string.Empty;
            var result = Ydu.Open((ushort)Convert.ToInt16(unitId), modelName);
            if(result == Ydu.YDU_RESULT_SUCCESS)
            {
                var str = "YduOpen success";
                Console.WriteLine(str);
                msg = str;
            }
            else
            {
                var str = string.Format("YduOpen error: 0x{0:X}", result);
                Console.WriteLine(str);
                msg = str;
            }

            return msg;
        }

        private string Close(ushort unitId)
        {
            var result = Ydu.Close(unitId);
            return "YduClose";
        }

    }

    public class DealSwitch : DealBase
    {
        public DealSwitch(Config config) : base(config)
        {
        }

        public override string GetResult(HttpListenerContext context, string Path)
        {
            var data = context.Request.QueryString["data"];
            var unitId = context.Request.QueryString["unitid"];
            var type = context.Request.QueryString["type"];
            var res = new PachinkoRes();
            //res.data = data;
            if(type == URL.PARAM_SWITCH_TYPE_ON)
            {
                res.Result = this.Switch(unitId, data, "1");
            }
            else if(type == URL.PARAM_SWITCH_TYPE_OFF)
            {
                res.Result = this.Switch(unitId, data, "0");
            }

            return JsonMapper.ToJson(res);
        }

        private string Switch(string unitId, string data, string outputNo)
        {
            byte[] outputData = new byte[1];
            ushort _outputNo;
            ushort _unitId;
            string msg = string.Empty;

            outputData[0] = Convert.ToByte(Convert.ToInt32(data, 16));
            _outputNo = Convert.ToUInt16(outputNo);
            _unitId = (ushort)Convert.ToInt16(unitId);

            var result = YduDio.Output(_unitId, outputData, _outputNo, 1);
            if(result == Ydu.YDU_RESULT_SUCCESS)
            {
                var str = "YduOutput success";
                Console.WriteLine(str);
                msg = str;
            }
            else
            {
                var str = string.Format("YduOutput error: 0x{0:X}", result);
                Console.WriteLine(str);
                msg = str;
            }

            return msg;
        }
    }

    public class DealLevel : DealBase
    {
        public DealLevel(Config config) : base(config)
        {
        }

        public override string GetResult(HttpListenerContext context, string Path)
        {
            var data = context.Request.QueryString["data"];
            var unitId = context.Request.QueryString["unitid"];

            var res = new PachinkoRes();

            res.Result = this.ChangeLevel(unitId, data, "1");
            //res.data = data;
            return JsonMapper.ToJson(res);
        }

        private string ChangeLevel(string unitId, string data, string outputData)
        {
            byte[] _outputData = new byte[1];
            ushort _outputNo;
            ushort _unitId;
            string msg = string.Empty;

            _unitId = (ushort)Convert.ToInt16(unitId);
            _outputData[0] = Convert.ToByte(outputData);
            _outputNo = Convert.ToUInt16(Convert.ToInt32(data, 16));
            Console.WriteLine("{0} {1} {2}", _unitId, _outputData[0], _outputNo);

            var result = YduDio.Output(_unitId, _outputData, _outputNo, 1);
            if (result == Ydu.YDU_RESULT_SUCCESS)
            {
                var str = "YduOutput success";
                Console.WriteLine(str);
                msg = str;
            }
            else
            {
                var str = string.Format("YduOutput error: 0x{0:X}", result);
                Console.WriteLine(str);
                msg = str;
            }

            return msg;
        }
    }

    public class DealInput : DealBase
    {
        public DealInput(Config config) : base(config)
        {
        }

        public override string GetResult(HttpListenerContext context, string Path)
        {
            var unitIdStr = context.Request.QueryString["unitid"];

            ushort unitId = (ushort)Convert.ToInt16(unitIdStr);
            byte[] inputData = new byte[16];
            ushort start = 0;
            ushort count = 16;

            var res = new InputRes();
            string msg = this.YduInput(unitId, inputData, start, count);
            res.Result = msg;
            res.InputData = inputData;
            return JsonMapper.ToJson(res);
        }
    }
}
