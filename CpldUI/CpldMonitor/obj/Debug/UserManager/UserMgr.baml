   M S B A M L   `   `   `  ����
  CpldUI     CpldUI.UserManager.UserMgr   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  用户管理��5   	   $	��400��6   $	��597��6#    � 
ResizeMode$ NoResize=�6/    � WindowStartupLocation$ CenterScreen=�6E    � Topmost .   � Icon$  /CpldUI;component/logo.ico��6�    � 	Resources 5         CenterAlignmentStyle �� 
TargetType �� Property �� Value��%d   &	        �� 5   
   " ��6-   .�5      ��  �@��TextAlignment$ Center=�6.   5      5	      .��5
      � ��5   
   �� 5      	 �� EndPoint$	 0.5,1&�6"   
 �� 
StartPoint$
 0.5,0&�63   .��5      �  � Color$ White��6    � Offset$ 0.419Q�6-   � 5      $ 	#FFF7EBFD��6   $ 1Q�61   5      .+�5   
   �� -   5        $��	groupBox1��5   \   $��用户账户��6   $	��337��6!   $
��Left=�6.   $��	12,12,0,0q�6I   $	��Top=�6m   $	��551��6�   $��16(�6�   .��5      � .+�5      `�  `� IsLocked . .�5      c� $	��540��6   $��50��6*   
�������.@�5      �� -   5         ��Name$ 
btnUserAdd��5   !    � ToolTip$ 新增用户信息��65   .��5      ��  �� Orientation$ 
Horizontal=�6)   .+�5   "   �� $
��None=�6(   $$��/CpldUI;component/Icon/add.png��67   �� 5   "   $��3,0,0,0q�6,   $��36��6=   $��23��6H   �� 6U   $�新增��6Y   6f   5      5      �� 5      $	�� | ��6    $��	3,5,10,10q�6.   �� 5      -   5        $ btnUserEdit��5   !   $ 修改用户信息��66   .��5      �� $ 
Horizontal=�6)   .+�5   "   �� $
��None=�6(   $'��!/CpldUI;component/Icon/modify.png��67   �� 5   "   $��3,0,0,0q�6,   $��37��6=   $��23��6H   �� 6U   $�修改��6Y   6f   5       5!      �� 5"      $	�� | ��6    $��	3,5,10,10q�6.   �� 5#      -   5        $ 
btnUserDel��5#   !   $ 删除用户信息��65   .��5$      �� $ 
Horizontal=�6)   .+�5%   "   �� $
��None=�6(   $$��/CpldUI;component/Icon/del.png��67   �� 5&   "   $��3,0,0,0q�6,   $��37��6=   $��24��6H   �� 6U   $�删除��6Y   6f   5'      5(      �� 5*      $	�� | ��6    $��	3,5,10,10q�6.   �� 5+      -   5        $ btnUserEdutAuthen��5+   !   $ 修改用户权限��6<   .��5,      �� $ 
Horizontal=�6)   .+�5-   "   �� $��25��6(   $��30��63   $
��Fill=�6?   $'��!/CpldUI;component/Icon/authen.png��6N   �� 5.   "   $��3,0,0,0q�6,   $	��103��6=   $��24��6I   �� 6V   $�修改用户权限��6Z   6k   5/      50      51      52      &   System.Windows.Controls.DataGrid  53      -   5        $��gridUserMgr��53   �      AutoGenerateColumns .  
�������   HeadersVisibility$ Column=�6J      CanUserAddRows .     
IsReadOnly . $	��259��6�   $
��Left=�6�   $��0,50,0,0q�6�   $	��Top=�6�   $	��539��6�   $��14(�6�      GridLinesVisibility$ 
Horizontal=�6     SelectionMode$ Single=�6.     FrozenColumnCount$ 3��6E     ColumnHeaderStyle 54      �� 55      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�56      ��  � ��$ Center=�6K   57      58         Columns 5:      0  *System.Windows.Controls.DataGridTextColumn  5;      ,  &System.Windows.Controls.DataGridColumn   Header$ 编号��6D      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6P   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6\      Binding 6-   �� *ID+6�     5<      $ 	用户名��6J   $	 200 6W   # [  6c    6-   �� *
UserName+6�     5=      $ 用户权限��6O   $	 237 6]   # [  6i    6-   �� *UserLevelName+6�   5?      5@      5A      5B      5C      