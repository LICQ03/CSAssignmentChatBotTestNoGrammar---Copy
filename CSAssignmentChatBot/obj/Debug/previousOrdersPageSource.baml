   M S B A M L   `   `   `  ����N  JCSAssignmentChatBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:CSAssignmentChatBotCSAssignmentChatBot  5   	   2    ,CSAssignmentChatBot.previousOrdersPageSource   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:CSAssignmentChatBot     5   	     � WindowStyle$
  None=�5       � AllowsTransparency . $	��450��5
      $	��800��6    � 
ResizeMode$ NoResize=�5       � Title$ previousOrdersPageSource��5   	   .��5      � 
�������.+�5   
   �� -   5        $��orderPreviousListBox��5      $y�Visible=�6/   $��Stretch=�6D   
�������$��	0,0,0,235q�6t    �� View 5      �� 5      .P�5      �� $��Order Number��6%    �� Width$	 100��6;    �� DisplayMemberBinding 6G   �� *orderNumber+�� 5      $
��Food��6%   $	 500��63    6?   �� *foods+�� 5      $��Total Cost ($)��6%   $	 110��6=    6I   �� *totalOrderedPrice+5      5"      �� 5#   
   -   5        $��questionText��5#   �   $��0,274,0,149q�6   $��Center=�6)   $��Center=�6D    �� TextAlignment$ Center=�6a   $
��Bold"�6x   $��20(�6�   $	��800��6�   6�   �� 5$   
   -   5        $��prevOrderTextBox��5$   [   $��240,315,240,105q�6   $��15(�6+   $��Center=�69   6u   �� 5%   
   -   5        $��prevOrderButton��5%   �   $��315,350,305,70q�6   $��Center=�6)   $��Center=�6K   
���   �$
��Bold"�6�   .��6�   Speak6�   �� 5&   
   -   5         ��Name$ prevOrderButton2��5&   �   $��315,350,305,70q�6   $��Center=�6)   $��Center=�6K   
���   �$y�	Collapsed=�6�   $
��Bold"�6�   $��Speak��6�   5,      5-      