   M S B A M L   `   `   `  ����
  CpldUI,    &CpldUI.RecordManager.CheckResultDetail   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Adx2http://schemas.devexpress.com/winfx/2008/xaml/core     5   	   -   5          � Title$  检测结果管理��5   	   $	��720��6   $
��1024��6%    � WindowStartupLocation$ CenterScreen=�62    � Icon$  /CpldUI;component/logo.ico��6n    � 	Resources 5         CenterAlignmentStyle �� 
TargetType �� Property �� Value��%d   &	        �� 5   
   " ��6-   .�5      ��  �@��TextAlignment$ Center=�6.   5	      5
      .��5      � ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      � 	 � Color$	 White��6   
 � Offset$
 0.419Q�6-   � 5      $	 	#ADD7EBFD��6   $
 1Q�61   5       � RowDefinitions 5   
   �� 5      $	��140 �6   �� 5      .+�5   
   �� -   5         ��Name$ 	groupBox1��5   /   $��筛选条件��6   $��16(�6!   .��5      � $��2,0,-2,0q�6   .+�5      (  "System.Windows.Controls.DatePicker  -   5          Name$ DatePickerBegin��5   W   $��29��6   $
��Left=�6)   $��	84,17,0,0q�6D   $	��Top=�6p   $	��140��6�   $��5��6�   �� 5      -   5         ��Name$ label5��5   a   $��起始日期:��6   $��29��6(   $
��Left=�64   $��6,17,0,0q�6O   $	��Top=�6q     5      -   5        $ DatePickerEnd��5   X   $��29��6   $
��Left=�6)   $��
335,17,0,0q�6D   $	��Top=�6o   $	��140��6�   $��6��6�   �� 5      -   5        $ label6��5   c   $��终止日期:��6   $��29��6(   $
��Left=�64   $��
253,17,0,0q�6O   $	��Top=�6s   �� 5      -   5         ��Name$ btnFilt��5   u   $��查询数据��6   |�.  $��30��6:   $
��Left=�6F   $��
511,16,0,0q�6a   $	��Top=�6�   $	��116��6�   ���	#FFFFF7556�   �� 5      -   5        $��CbCheckResult��5       �� SelectedIndex$ 0��60   $��30��6B   $
��Left=�6N   $��	84,64,0,0q�6i   $	��Top=�6|   $	��140��6�   ��6�   Z� .@�5      �� .��6#   全部6'   �� 5      .��6#   OK6'   �� 5       .��6#   NG6'   5!      �� 5"      -	   5        $ label5_Copy��5"   a   $��检测结果:��6   $��29��6(   $
��Left=�64   $��6,65,0,0q�6O   $	��Top=�6v   �� 5#      -
   5        $ 	btnExport��5#   x   $��导出EXCEL��6   |�.  $��30��6=   $��Right=�6I   $��	0,17,20,0q�6e   $	��Top=�6�   $	��116��6�   ���	#B22074466�   5$      5%      �� 5'   
   $��1��6   $��检测数据��6    $��16(�6.   .��5(      � .+�5)      &   System.Windows.Controls.DataGrid  -   5        $��GridCheckResult��5)   J   
�������   AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    SelectionMode$ Single=�6�      ColumnHeaderStyle 5*      �� 5+      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�5,      ��  � ��$ Center=�6K   6\   5-         Columns 50      0  *System.Windows.Controls.DataGridTextColumn  51      ,  &System.Windows.Controls.DataGridColumn   Header$ 创建日期��6K      Width5  /System.Windows.Controls.DataGridLengthConverter$
 100* 6Y      MinWidth$	 100Q�6f   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6u      Binding 6-   �� *	CheckDate+6�     52      $ 	零件号��6J   $
 100* 6W   $	 100Q�6d   # [  6s    6-   �� *
ItemName+6�     53      $ 	检测员��6J   $
 100* 6W   $	 100Q�6d   # [  6s    6-   �� *
Username+6�     54      $ 扫描信息��6L   $
 100* 6Z   $	 100Q�6g   # [  6v    6-   �� *
BarContent+6�     55      $ 检测结果��6G   $
 100* 6U   $	 100Q�6b   # [  6q    6-   �� *State+6�   57      58      59      5:      5;      