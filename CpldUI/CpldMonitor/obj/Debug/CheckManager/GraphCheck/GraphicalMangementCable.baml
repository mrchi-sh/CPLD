   M S B A M L   `   `   `  ����A  =CpldUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:CpldUI.CheckManager.GraphCheckCpldUI.CheckManager.GraphCheck  5   	   <    6CpldUI.CheckManager.GraphCheck.GraphicalMangementCable   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   >local,clr-namespace:CpldUI.CheckManager.GraphCheck     5   	     � Title$  选择产品��5   	   $	��700��6   $
��1024��6#   $	��768��60   $
��1024��6@    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$  /CpldUI;component/logo.ico��6.    � 	Resources 5         CenterAlignmentStyle �� 
TargetType �� Property �� ValueE   ?CpldUI.CheckManager.GraphCheck.GraphicalMangementCableViewModel  	ViewModel��%�   &	        &	 Z     �� 5   
   " ��6-   .�5	      ��  �@��TextAlignment$ Center=�6.   5
        5   
   61   5      .��5      � -   5        $
��Grid��5       � DataContext 6   ��  �� Source# [ ��5   
   �� 5      	 �� EndPoint$	 0.5,1&�6"   
 �� 
StartPoint$
 0.5,0&�63   .��5      �  � Color$ White��6    � Offset$ 0.419Q�6-   � 5      $ 	#ADD7EBFD��6   $ 1Q�61   5       � RowDefinitions 5   
   �� 5      $��8* �6   �� 5      $��70 �6   .+�5   
   �� $��产品信息��6   $��16(�6!   $��0��6/   .��5      � ���	#FFE6FFFD 5      �� 5      $��50 �6$   �� 5      $��1* �6$   .+�5      � $��0��6    � ColumnDefinitions 5      �� 5       $��80 �6+   �� 5!      $��1* �6+   �� 5"      $	��100 �6+   .+�5$      �� $��0��6   $��16(�6,   $��
产品名:��6:   $
��Left=�6I   $��Center=�6d   �� 5%      $��1��6   $��16(�6.   $
��0,10q�6<    �� TextWrapping$
 Wrap=�6J   ��6^   �� *
SearchText+�� 5&      -   5        $��2��5&      $
��0,10q�6-   ��6;   Z� .��5'      ��  �� Orientation$ 
Horizontal=�6%   .+�5(      �� $'��!/CpldUI;component/Icon/search.png��6$   $
��fill=�6O   $��24��6^   $��23��6j   �� 5)      $��16(�6(   $��3,-1,0,0q�66   $��37��6H   $��21��6S   �� 6`   $�查找��6d   6q   5*      5+      5,      &   System.Windows.Controls.DataGrid  5-      -   5        $��1��5-      
�������$��0,0,0,0q�5.   .      AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly .    SelectionMode$ Single=�51   -   ��52      �� *	CableList+ �� SelectedItem 53      �� *SelectedCable+   ColumnHeaderStyle 55      �� 56      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�57      ��  � ��$ Center=�6K   58         Columns 5:      0  *System.Windows.Controls.DataGridTextColumn  5;      ,  &System.Windows.Controls.DataGridColumn   Header$ 创建日期��6L      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6Z   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6g      Binding 6-   �� *
ModifyDate+6g     5<      $ 产品编号��6K   $
 150* 6Y   # [  6f    6-   �� *	CableName+6f     5=      $ 	产品名��6H   $
 150* 6U   # [  6b    6-   �� *Remark+6b   5?      5@      5A      � 5B   
   $��1��6    5C      �� 5D      $��2* �6#   �� 5E      $��1* �6#   �� 5F      $��1* �6#   �� 5G      $��1* �6#   �� 5H      $��1* �6#   �� 5I      $��1* �6#   �� 5J      $��3* �6#   �� 5K      $��2* �6#   �� 5L      $��2* �6#   .+�5N      �� -   5        $��0��5O      $��20q�5P      $��16(�5Q      ���	#FFFFC04E{�5N      �� *IsBarEditEnable+.��5T      �� $ 
Horizontal=�6   .+�5U      �� $
��Fill=�6   $��20��6+   $��32��67   $*��$/CpldUI;component/Icon/bartender.png��6B   �� 5V      $��22��6    �� 6-   $� 条码配置��61   6B   5W      5X      �� 5Y      -   5        $��新增��5Y      $��16(�5[      ���	#FFFFC04E$
��5,20q�5]      $��1��5^      {�5Z      �� *IsCableEditEnable+5_      �� 5`      -   5        $��删除��5`      $��16(�5b      $
��5,20q�5c      ���	#FFFFC04E$��2��5e      {�5a      �� *IsCableEditEnable+5f      �� 5g      -   5        $��复制��5g      $
��5,20q�5i      $��3��5j      $��16(�5k      ���	#FFFFC04E{�5h      �� *IsCableEditEnable+5m      �� 5n      -   5        $��修改��5n      $
��5,20q�5p      $��4��5q      $��16(�5r      ���	#FFFFC04E{�5o      �� *IsCableEditEnable+5t      �� 5u      -	   5        $��绘图��5u      $
��5,20q�5w      $��5��5x      $��16(�5y      ���	#FFFFC04E{�5v      �� *IsCableEditEnable+5{      �� 5|      -
   5        $��16(�5|      $
��5,20q�5}      
������$��7��5      $��确认��5�      5�      �� 5�      $��16(�6   $
��5,20q�5�      
�������$��关闭��5�      $��8��5�       �� IsCancel . 5�      5�      5�      5�      