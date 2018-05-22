namespace CpldBase.DataStructure
{
    public class TagCommandResult
    {
        public byte u8_is_ok { get; set; }
        public byte u8_result1 { get; set; }
        public byte[] u16_result2 { get; set; }
        public byte[] u32_append_data_size { get; set; }
    }

    public class TagLineAddr
    {
        public byte u8_board_addr { get; set; }
        public byte u8_index_on_board { get; set; }
    }

    public class TagBoardLinesState
    {
        public byte u8_board_addr { get; set; }
        public byte[] u8_reserved { get; set; }
        public byte[] ary_u8_checked_flag { get; set; }
        public TagLineAddr[] ary_short_circuit_info { get; set; }
    }

    public class TagShortCircuitInfo 
    {
        public byte u8_cpld_count { get; set; }
        public byte[] u8_reserved { get; set; }
        public byte[] u16_short_circuit_group_count { get; set; }
        public byte[] u16_short_lines_count { get; set; }
        public TagBoardLinesState[] ary_cpld_state { get; set; }
    }

    
}
