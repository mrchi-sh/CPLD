   M S B A M L   `   `   `  ����
  CpldUI-    'CpldUI.RecordManager.CheckResultManager   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Adx2http://schemas.devexpress.com/winfx/2008/xaml/core     5   	   -   5          � Title$  检测结果管理��5   	   $	��800��6   $
��1024��6%    � WindowStartupLocation$ CenterScreen=�62    � Icon$  /CpldUI;component/logo.ico��6�    � 	Resources 5         CenterAlignmentStyle �� 
TargetType �� Property �� Value��%d   &	        �� 5   
   " ��6-   .�5      ��  �@��TextAlignment$ Center=�6.   5	      5
      .��5      � ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      � 	 � Color$	 White��6   
 � Offset$
 0.419Q�6-   � 5      $	 	#ADD7EBFD��6   $
 1Q�61   5       � RowDefinitions 5   
   �� 5      $	��128 �6   �� 5      .+�5   
   �� -   5         ��Name$ 	groupBox1��5   /   $��筛选条件��6   $��16(�6!   .��5      � $��2,0,-2,0q�6   .+�5      �� -   5         ��Name$ label1��5   `   $��品名:��6   $��29��6&   $
��Left=�62   $��	27,15,0,0q�6M   $	��Top=�6p   �� 5      -   5         ��Name$ 
tbItemName��5   \   $��30��6   $�� ��6&   $
��Left=�6.   $��	75,14,0,0q�6I   $	��Top=�6p   $	��140��6�   $��0��6�   6�   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ datePickerBegin��5   W   $��29��6   $
��Left=�6)   $��	75,60,0,0q�6D   $	��Top=�6p   $	��140��6�   $��5��6�   �� 5      -   5        $ label5��5   b   $��起始日期:��6   $��29��6(   $
��Left=�64   $��	-3,60,0,0q�6O   $	��Top=�6r     5      -   5        $ datePickerEnd��5   X   $��29��6   $
��Left=�6)   $��
326,60,0,0q�6D   $	��Top=�6o   $	��140��6�   $��6��6�   �� 5      -   5        $ label6��5   c   $��终止日期:��6   $��29��6(   $
��Left=�64   $��
244,60,0,0q�6O   $	��Top=�6s   �� 5      -	   5         ��Name$ btnFilt��5   u   $��查询数据��6   |�.  $��30��6:   $
��Left=�6F   $��
735,17,0,0q�6a   $	��Top=�6�   $	��116��6�   ���	#FFFFF7556�   �� 5      -
   5        $ 	btnExport��5   x   $��导出EXCEL��6   |�.  $��30��6=   $��Right=�6I   $��	0,17,20,0q�6e   $	��Top=�6�   $	��116��6�   ���	#B22074466�   5       5!      �� 5#   
   $��1��6   $��检测数据��6    $��16(�6.   .��5$      � .+�5%      &   System.Windows.Controls.DataGrid  -   5        $��GridCheckResult��5%   J   
�������   AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    SelectionMode$ Single=�6�      ColumnHeaderStyle 5&      �� 5'      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�5(      ��  � ��$ Center=�6K   5)      5*         	CellStyle 5,      �� 5-      *  $System.Windows.Controls.DataGridCell"  6    .�5.      ��  �@^�ToolTip$$ 双击获取详细检测信息��6G   5/         Columns 52      0  *System.Windows.Controls.DataGridTextColumn  53      ,  &System.Windows.Controls.DataGridColumn   Header$ 检测类型��6M      Width5  /System.Windows.Controls.DataGridLengthConverter$
 100* 6[   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6h      Binding 6-   �� *CheckMethod+6�     54      $ 品名��6J   $
 200* 6V   # [  6c    6-   �� *
ItemName+6�     55      $ OK数量��6H   $
 150* 6V   # [  6c    6-   �� *OkTime+6�     56      $ NG数量��6H   $
 150* 6V   # [  6c    6-   �� *NgTime+6�   58      59      5:      5;      5<      