   M S B A M L   `   `   `  ����
  CpldUI#    CpldUI.CheckManager.SelfCheck   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  自检��5   	   $	��400��6   $	��600��6!    � 
ResizeMode$ NoResize=�6-    � WindowStartupLocation$ CenterScreen=�6C    � Icon$  /CpldUI;component/logo.ico��6h    � 	Resources 5         CenterAlignmentStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5   
   " ��6-   .�5      ��  �@��TextAlignment$ Center=�6.   �� 5       � ��$ WordEllipsis=�6-   5	      5
      .��5      � ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"   	 �� 
StartPoint$	 0.5,0&�63   .��5      � 
 � Color$
 White��6    � Offset$ 0.419Q�6-   � 5      $
 	#ADD7EBFD��6   $ 1Q�61   5       � RowDefinitions 5   
   �� 5      $
��7.5* �6   �� 5      $��2* �6   .+�5   
   �� $��自检结果��6   $��16(�6!   $��0��6/   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ GridSelfCheck��5      
�������   AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    SelectionMode$ Single=�5   �   $	��0,5q�6�    � DataContext 6F   ��    ColumnHeaderStyle 5      �� 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  � ��$ Center=�6G   5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        ColumnId��,  &System.Windows.Controls.DataGridColumn   Header$ 编号��5   R      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 5    )   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  66      Binding 5   ;   �� *Id+5    6     5!      -   5        ColumnResult��$ 短路节点��5!   _   $
 150* 5"   )   # [  66    5!   ?   �� *CheckResult+5"   6   5$      5%      � 5&   
   $��1��6    � ColumnDefinitions 5'      �� 5(      $��2* �6#   �� 5)      $��1* �6#   �� 5*      $��1* �6#   �� 5+      $��1* �6#   .+�5-      �� $��拔下电缆点击开始��6   $��20(�6'   $��10,20q�65   
���  ��
��� ���$��Center=�5/      $��Center=�60   �� 50      -   5        $��BtnPointSearch��52   $   $��16(�50      $��1��6#   $��10,20q�64   
��� ��� �� 	IsDefault . $��	点搜索��52      6:   �� 53      -   5        $��BtnOk��55   #   $��16(�53      $��2��6#   $��10,20q�64   
������ . $��开始��55      60   �� 56      $��16(�6   $��3��6#   $��10,20q�63   
������� �� IsCancel . $��关闭��58      59      5:      5;      