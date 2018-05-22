namespace CpldBase.DataStructure
{
    public class TagCommandRequest
    {
        public byte u8_cmd_id { get; set; }
        public byte u8_para1 { get; set; }
        public byte[] u16_para2 { get; set; }
        public byte[] u32_para3 { get; set; }
        public byte[] s_buffer { get; set; }
    }
}
