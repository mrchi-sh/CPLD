namespace CpldControl
{
    public enum CommandType
    {
        SampleCheck,
        BoardCount,
        PointCheck,
        OpenRelay,
        CloseRealy,
    }

    internal class CommandData
    {
        internal static byte[] GetCommandData(CommandType type)
        {
            var tmpCmd = new CpldBase.DataStructure.TagCommandRequest
            {
                u16_para2 = new byte[2],
                u32_para3 = new byte[4],
                s_buffer = new byte[20]
            };
            switch (type)
            {
                case CommandType.SampleCheck:
                    tmpCmd.u8_cmd_id = 0x01;                   
                    return DataParser.CommonParser.StructToByte(tmpCmd);

                case CommandType.BoardCount:
                    tmpCmd.u8_cmd_id = 0x02;
                    return DataParser.CommonParser.StructToByte(tmpCmd);

                case CommandType.PointCheck:
                    tmpCmd.u8_cmd_id = 0x03;
                    return DataParser.CommonParser.StructToByte(tmpCmd);

                case CommandType.OpenRelay:
                    tmpCmd.u8_cmd_id = 0x04;
                    tmpCmd.u8_para1 = 0x01;
                    return DataParser.CommonParser.StructToByte(tmpCmd);
                case CommandType.CloseRealy:
                    tmpCmd.u8_cmd_id = 0x04;
                    return DataParser.CommonParser.StructToByte(tmpCmd);
                default:
                    return null;
            }
        }
    }
}
