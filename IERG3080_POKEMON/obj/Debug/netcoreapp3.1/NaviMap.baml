   M S B A M L   `   `   `  ����K  GIERG3080_POKEMON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:IERG3080_POKEMONIERG3080_POKEMON  5   	       IERG3080_POKEMON.NaviMap   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:IERG3080_POKEMON     5   	   -   5          � Title$  NaviMap��5   	   $	��500��6   $	��800��6&    � 	Resources 5
         ListBox.Static.Background �� Color  ListBox.Static.Border  ListBox.Disabled.Background  ListBox.Disabled.Border  ListBoxStyle1 �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Padding	 J� SnapsToDevicePixels
 �� Triggers P� Property P� Value �� 
TargetName ]� 
Conditions �� Property �� Value��%�  &	        &	 #     &	 F     &	 i     &	 �     7   7  7  7  �� 5   
   $ 	#FFFFFFFF��6<   �� 5   
   $ 	#FFABADB3��68   �� 5   
   $ 	#FFFFFFFF��6>   �� 5   
   $ 	#FFD9D9D9��6:   �� 5   
   " ��6&   .�5      ��  � ��8   6+   �� 5       � ��8  6,   �� 5       � ��$ 1q�60   �� 5       � ��# � ��6+   �� 5       � ��$
 Auto=�6K   �� 5       � ��$
 Auto=�6I   �� 5       � ��$
 true��6>   �� 5       �@��PanningMode$
 Both=�69   �� 5       �@��IsFlicksEnabled$ False��67   �� 5       � ��$ Center=�69   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ Bd��6!   #��z��6-   #��z��6W   #��z��6�   $ 1q�6�   $
	 true��6�   ._�5      �� $|�false��6+   #��z��6=   .��5   "   �� #	 z	 61   $��LeftToRight=�6m   5       5!      
 5"      P� 5#       � {�$ false��6;   . �5$   "   �� $ Bd��6?   8   � ��6O   �� 5%   "   $ Bd��6@   8   � ��6P   5&      ]� 5'       5(   "   �� 5)   &    �@��
IsGrouping$
 true��6F   �� 5*   &   " �@%�IsVirtualizingWhenGrouping$ false��6h   ./�5,   "   ��  � ��$ false��6R   5-      5/      51      52      53      ��54      �� 55   
    �� ImageSource$; 5/IERG3080_POKEMON;component/img_navimap/wallpaper.jpg��6    j� Stretch$ UniformToFill=�6Y    j� TileMode$
 None=�6q    �� Opacity$	 0.9Q�6�   .��57      � .+�58   
   �� $��Center=�6   $	��400��6.   $��Center=�6;   $	��400��6V   .+�59      � -   5         �Name$ WP��59      $	��400��6$   $	��400��61   � 5:      -   5        $ WP_PKM��5:      $	��400��6(   $	��400��65   � 5;      -   5        $	 WP1��5;      $	��400��6%   $	��400��62   5<      �� 5=   
   -   5         ��Name$ UP��5=      $��W��6   $
��Left=�6)   $��
80,352,0,0q�6D   $	��Top=�6X   $��32��6p   $��34��6|   $��Adobe Gothic Std B)�6�   �� 5>   
   -   5        $
 LEFT��5>      $��A��6   $
��Left=�6+   $��
48,384,0,0q�6F   $	��Top=�6Z   $��34��6r   $��32��6~   $��Adobe Gothic Std B)�6�   6�   �� 5?   
   -   5        $ RIGHT��5?      $��D��6    $
��Left=�6,   $��114,384,0,0q�6G   $	��Top=�6\   $��34��6t   $��32��6�   $��Adobe Gothic Std B)�6�   6�   �� 5@   
   -   5        $
 DOWN��5@      $��S��6   $
��Left=�6+   $��
80,418,0,0q�6F   $	��Top=�6Z   $��32��6r   $��34��6~   $��Adobe Gothic Std B)�6�   6�   �� 5A   
   -	   5         ��Name$ PokeShowBox��5A   :   #���  6   $��613,27,19,299q�6O   $��Visible=�6f    �� SelectionMode$ Single=�6�   ��5B      �� 5C      .R�5D      �� ��5E      `� 5F       �� StringFormat$	 {0}��6+   .1�5G   "   ��  �� Path$ DisplayName�6*   5H      5J      5K      5M      �� 5N   
   -
   5        $ ViewPoke��5N      $
��View��6#   $
��Left=�62   $��629,239,0,0q�6M   $	��Top=�6b   $��52��6z   $��41��6�   $��14(�6�   $��Normal$�6�   $
��Bold"�6�   $��Arial Rounded MT Bold)�6�   �� 5O   
   -   5        $ 
RenamePoke��5O      $��Rename��6%   $
��Left=�66   $��686,239,0,0q�6Q   $	��Top=�6f   $��83��6~   $��41��6�   $
��Bold"�6�   $��Arial Rounded MT Bold)�6�   $��14(�6�   �� 5P   
   -   5        $ SellPoke��5P      $
��Sell��6#   $
��Left=�62   $��666,421,0,0q�6M   $	��Top=�6b   $��32��6z   $��58��6�   
���  ��$��15(�6�   $��
ExtraBlack"�6�   $��Normal$�6�   $��Arial Rounded MT Bold)�6�   �� 5Q   
   -   5        $ 
PotionPoke��5Q      $��
Use Potion��6%   $
��Left=�6:   $��628,299,0,0q�6U   $	��Top=�6j   $	��141��6�   $��40��6�   $��14(�6�   $
��Bold"�6�   $��Arial Rounded MT Bold)�6�   �� 5R   
   -   5         ��Name$ NicknamePoke��5R      $
��Left=�6*   $��
18,130,0,0q�6E   $��
Nickname: ��6Y   $
��Wrap=�6k   $	��Top=�6   
�������$	��108��6�   $��Arial Rounded MT Bold)�6�   �� 5S   
   -   5        $ HPPoke��5S      $
��Left=�6$   $��131,130,0,0q�6?   $
��HP: ��6T   $
��Wrap=�6`   $	��Top=�6t   
�������$��50��6�   $��LeftToRight=�6�   $��Arial Rounded MT Bold)�6�   �� 5T   
   -   5        $ 	LevelPoke��5T      $
��Left=�6'   $��
18,178,0,0q�6B   $��Level: ��6V   $
��Wrap=�6e   $	��Top=�6y   
�������$��80��6�   $��Arial Rounded MT Bold)�6�   �� 5U   
   -   5        $ Battle_ready��5U      $��Battle Ready��6'   $
��Left=�6>   $��628,359,0,0q�6Y   $	��Top=�6n   $	��141��6�   $��39��6�   $��14(�6�   $
��Bold"�6�   $��Arial Rounded MT Bold)�6�   �� 5V   
   -   5         ��Name$ ImgPoke��5V      $
��Left=�6!   $	��110��6<   $��	43,10,0,0q�6I   $	��Top=�6\   $	��110��6t   $��UniformToFill=�6�   �� 5W   
   -   5        $ Battle_status��5W      $
��Left=�6+   $��103,178,0,0q�6F   $��battle ready? ��6[   $
��Wrap=�6q   $	��Top=�6�   
�������$��78��6�   $��15��6�   $��Arial Rounded MT Bold)�6�   �� 5X   
   -   5        $ Potion_status��5X      $
��Left=�6+   $��642,205,0,0q�6F   $��Number of Potions��6[   $
��Wrap=�6t   $	��Top=�6�   
�������$	��114��6�   $��20��6�   $��Arial Rounded MT Bold)�6�   �� 5Y   
   -   5        $ BattleListBox��5Y      $��29,205,626,146q�6)   ��5Z      �� 5[      .R�5\      �� ��5]      `� 5^      $ {0}   PkmID: {1}��6+   .1�5_   "   �� $ DisplayName�6*   �� 5`   "   $ ID�6*   5a      5c      5d      5f      �� 5g   
   -   5        $ EXPinfo��5g      $
��Left=�6%   $��
18,154,0,0q�6@   $��Exp: ��6T   $
��Wrap=�6a   $	��Top=�6u   
�������$	��163��6�   $��Arial Rounded MT Bold)�6�   5h      5i      