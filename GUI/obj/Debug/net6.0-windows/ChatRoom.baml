   M S B A M L   `   `   `  ����>  :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI  5   	       GUI.ChatRoom   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:GUI     5   	     � Title$  ChatRoom��5   	   $	��450��6   $	��800��6'   .��5	      �    Main#���   6    � RowDefinitions 5
   
   �� 5      $
��400* �6   �� 5      $	��40* �6    � ColumnDefinitions 5   
   �� 5      $	��15* �6   �� 5      $
��138* �6   �� 5      $	��12* �6   �� 5      $	��35* �6   .+�5   
   � $��Stretch=�6   $��5,5,2,5q�62    � Orientation$ Vertical=�6C   $��Stretch=�6Z   .+�5      �� -   5         ��Name$ 
ExitButton��5      $��E��6)     	Highlight#���  65   $��Center=�6^   $��Center=�6�   $��	Wingdings)�6�   $��30(�6�   $
��Bold"�6�   $��Normal$�6�   $��0,0,0,0q�6�   $��53��6   $��39��6  5      �� 5   
   $��5,5,2,5q�6   
���   �$��2��64   �� 5   
   $��2,5,2,2q�6   
���   �$��1��64   $��2��6D   �� 5    
   $��2,5,5,2q�6   
���   �$��3��64   �� 5"   
   $��2,2,2,5q�6   
���   �$��1��64   $��1��6D   �� 5#   
   -   5         ��Name$ MessageInput��5#      $��2,2,2,5q�6,   $��1��6=   $��1��6M   �� 5%   
   $��2,2,5,5q�6   
���   �$��3��64   $��1��6D   �� 5(   
   $��2,2,2,5q�6   
���   �$��2��64   $��1��6D   �� 5)   
   -   5        $ 
SendButton��5)      $��2��6%   $
��Send��65   $��Stretch=�6D   $��2,2,2,5q�6b   $��1��6s   $��Stretch=�6�   #���  6�   �� 5+   
   -   5         ��Name$ MessageContainer��5+      $��2,2,2,5q�6,   $��1��6=   $��2��6M    � ContextMenu 5,      �� 5-       �� 	StaysOpen$
 true��6   .@�5.      m� -   5        $	��Add��5.      6,   m� 5/      -   5        $��Delete��5/      6/   m� 50      -   5        $
��Edit��50      6-   51      53      �� 55   
   -   5        $ MemberContainer��55      $��2,2,2,5q�6+   $��3��6<   $��1��6L   56      5:      5;      