   M S B A M L   `   `   `  ���� >  :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI      GUI.ChatRoom   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     #localclr-namespace:GUI       � Title$  ChatRoom��$	��450��$	��800��.���    Main#���    � RowDefinitions �� $
��400* ��� $	��40* � � ColumnDefinitions �� $	��15* ��� $
��138* ��� $	��12* ��� $	��35* �.+�� $��Stretch=�$��5,5,2,5q� � Orientation$ Vertical=�$��Stretch=�.+��� -    ��Name$ 
ExitButton��$��E��  	Highlight#���  $��Center=�$��Center=�$��	Wingdings)�$��30(�$
��Bold"�$��Normal$�$��0,0,0,0q�$��53��$��39���� $��5,5,2,5q�
���   �$��2���� $��2,5,2,2q�
���   �$��1��$��2���� $��2,5,5,2q�
���   �$��3���� $��2,2,2,5q�
���   �$��1��$��1���� -    ��Name$ MessageInput��$��2,2,2,5q�$��1��$��1���� $��2,2,5,5q�
���   �$��3��$��1���� $��2,2,2,5q�
���   �$��2��$��1���� -   $ 
SendButton��$��2��$
��Send��$��Stretch=�$��2,2,2,5q�$��1��$��Stretch=�#���  �� -    ��Name$ MessageContainer��$��2,2,2,5q�$��1��$��2�� � ContextMenu ��  �� 	StaysOpen$
 true��.@�m� -   $	��Add��m� -   $��Delete��m� -   $
��Edit���� -   $ MemberContainer��$��2,2,2,5q�$��3��$��1��