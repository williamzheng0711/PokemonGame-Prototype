   M S B A M L   `   `   `  ����K  GIERG3080_POKEMON, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:IERG3080_POKEMONIERG3080_POKEMON  5   	        IERG3080_POKEMON.GymBattle   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:IERG3080_POKEMON     5   	     � Title$  	GymBattle��5   	   $	��500��6   $	��800��6(    � WindowStyle$
 None=�64    � 	Resources 5	         ProgressBar.Progress �� Color  ProgressBar.Background  ProgressBar.Border  RoundedHPBar �� 
TargetType �� Property �� Value �� 
TargetType �Name	 � ColumnDefinitions'  !System.Windows.VisualStateManager
   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard g� RepeatBehavior �� 
TargetName �� TargetProperty9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value8  2System.Windows.Media.Animation.EasingPointKeyFrame $� KeyTime $� Value �� CornerRadius ��Name J� RenderTransformOrigin �� Triggers P� Property P� Value �� 
TargetName �� Angle��%�  &	        &	 #     &	 F     &	 i     7   7  7  �� 5
   
   $ 	#FF06B025��67   �� 5   
   $ 	#FFE6E6E6��69   �� 5   
   $ 	#FFBCBCBC��65   �� 5   
   " �6%   .�5      ��  � ��8   6+   �� 5       � ��8  6+   �� 5       � ��8  6,   �� 5       � ��$ 1q�60   �� 5       � �� 5      �� 5      " �6&   .R�5      � $ TemplateRoot��6   	 5      �� 5   "   $	��47* �63   �� 5   "   $	��33* �63   
 5        5   "   CommonStates��. 5   &     Determinate��62     5   &   Indeterminate��. 5   *   �� $ Forever��65   .��5   .   U� $ 	Animation��6L   $V P(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)�6n   .\�5    2     $ 0��6G   $
 0.25Q�6S     5!   2   $ 0:0:1��6G   $
 0.25Q�6W     5"   2   $ 0:0:2��6G   $
 0.25Q�6W   5#   /   *� 5$   .   $ 	Animation��6K   $' !(UIElement.RenderTransformOrigin)�6m   .'�5%   2     $ 0��6F   $ -0.5,0.5&�6R     5&   2   $ 0:0:1��6F   $ 0.5,0.5&�6V     5'   2   $ 0:0:2��6F   $ 1.5,0.5&�6V   5(   /   5)   +   5*   '   5+   #   .+�5-      �� #��z��6%   #��z��6O   #��z��6�   $ 10��6�   $��2��6�   �� 5.      $ 
PART_Track��6(   $��2��6<   � 5/      $ PART_Indicator��6#   $
}�true��6;   $��2��6O   .+�50   "   �� $ 	Indicator��6,   #��z��6?   �� 51   "   $ 	Animation��6,   #��z��6?   $ 0.5,0.5&�6c   z�52   &   V� 53   *   .~�54   .   �� �� 55   .   �� 56   .   T� 57   .   58   +   5:   #   5;      5<       5=      P� 5>       �@�Orientation$ Vertical=�6=   . �5?   "   �� $ TemplateRoot��6D    �@�LayoutTransform 5@   &   �� 5A   *   $	 -90Q�6:   5C   #   5D      P� 5E       �@�IsIndeterminate$
 true��6A   . �5F   "   �� $ 	Indicator��6?    � y�$ 	Collapsed=�6V   5G      5I      5K      5L      5M      .��5N      � ��5O   
   �� 5P       �� ImageSource$  img_gymbattle/battlebg.png��6    j� Stretch$
 Fill=�6C    �� Opacity$	 0.9Q�6R   .+�5R   
   �� -   5         ��Name$ YourPokemon��5R      $
��Left=�6%   $	��183��6@   $��
148,94,0,0q�6M   $	��Top=�6a   $	��183��6y   �� 5S   
   -   5        $ EnemyPokemon��5S      $
��Left=�6&   $	��183��6A   $��
428,94,0,0q�6N   $	��Top=�6b   $	��183��6z   � 5T   
   -   5          �Name$  
EnemyHpBar��5T      $
��Left=�6*   $��28��6E   $��
414,25,0,0q�6Q   $	��Top=�6e   $	��320��6}   ! � Value$	! 100Q�6�   " � UseLayoutRounding$
" True��6�   # J� Opacity$# 0Q�6�   $��RightToLeft=�6�   ��5U      �� 5V      $ 	OrangeRed��6"   5X      � 5Y   
   -   5        $  MyHpBar��5Y      $
��Left=�6'   $��28��6B   $��	41,25,0,0q�6N   $	��Top=�6a   $	��320��6y   $	! 100Q�6�   $
" True��6�   $# 0Q�6�   ��5Z      �� 5[      $ 	OrangeRed��6"   5]      �� 5^   
   -   5        $ ��Name$$ START_FIGHT��5^      $��Start Fight��6&   $
��Left=�6<   $��
648,94,0,0q�6W   $	��Top=�6k   $��42��6�   $	��109��6�   $��15(�6�   $��Arial Rounded MT Bold)�6�   �� 5_   
   -   5        $$ leave��5_      $��Leave��6    $
��Left=�60   $��648,158,0,0q�6K   $	��Top=�6`   $��41��6x   $	��109��6�   $��15(�6�   $��Arial Rounded MT Bold)�6�   �� 5`   
   -   5        $$ Skill1��5`      $��Light Attack��6!   $
��Left=�68   $��
51,366,0,0q�6S   $	��Top=�6g   $��80��6   $	��250��6�   $��20(�6�   $
��Bold"�6�   $��Arial Rounded MT Bold)�6�   ${�False��6�   �� 5a   
   -   5        $$ Skill2��5a      $��Heavy Attack��6!   $
��Left=�68   $��324,366,0,0q�6S   $	��Top=�6h   $��80��6�   $	��250��6�   $��20(�6�   $��Arial Rounded MT Bold)�6�   ${�False��6�   �� 5b   
   -	   5        % ��Name$% 	LabelText��5b      $&�� Choose a skill to attack enemy !��6#   $
��Left=�6N   $��
51,312,0,0q�6i   $	��Top=�6}   $��Arial Rounded MT Bold)�6�   $��25(�6�   
�������6�   �� 5c   
   -
   5        $$ ChangePokemon��5c      $��Change��6(   $
��Left=�69   $��661,223,0,0q�6T   $	��Top=�6i   $��42��6�   $��84��6�   $��15(�6�   $��Arial Rounded MT Bold)�6�   �� 5d   
   -   5        & ��Name$& ChargeCalculator��5d      $
��Left=�6.   $��358,419,0,0q�6I   $'��!Light attack charges heavy attack��6^   $
��Wrap=�6�   $	��Top=�6�   $
��Bold"�6�   $��Adobe Gothic Std B)�6�   �� 5e   
   -   5        $$ Pokemon1��5e      $�� ��6#   $
��Left=�6.   $��636,320,0,0q�6I   $	��Top=�6^   $��55��6v   $��55��6�   ${�False��6�   6�   �� 5f   
   -   5        $$ Pokemon2��5f      $�� ��6#   $
��Left=�6.   $��636,391,0,0q�6I   $	��Top=�6^   $��55��6v   $��55��6�   ${�False��6�   6�   �� 5g   
   -   5        $& HPPoke1��5g      $
��Left=�6%   $��711,328,0,0q�6@   $
��HP: ��6U   $
��Wrap=�6a   $	��Top=�6u   
�������$��64��6�   $��LeftToRight=�6�   $��Arial Rounded MT Bold)�6�   $��16��6�   $	# 0.2Q�6�   $��8(�6  �� 5h   
   -   5        $& HPPoke2��5h      $
��Left=�6%   $��711,396,0,0q�6@   $
��HP: ��6U   $
��Wrap=�6a   $	��Top=�6u   
�������$��63��6�   $��LeftToRight=�6�   $��Arial Rounded MT Bold)�6�   $��18��6�   $	# 0.2Q�6�   $��8(�6  �� 5i   
   -   5        $& 
LevelPoke1��5i      $
��Left=�6(   $��709,350,0,0q�6C   $��Level: ��6X   $
��Wrap=�6g   $	��Top=�6{   
�������$��66��6�   $��Arial Rounded MT Bold)�6�   $��18��6�   $	# 0.2Q�6�   �� 5j   
   -   5        $& 
LevelPoke2��5j      $
��Left=�6(   $��708,420,0,0q�6C   $��Level: ��6X   $
��Wrap=�6g   $	��Top=�6{   
�������$��66��6�   $��Arial Rounded MT Bold)�6�   $��20��6�   $	# 0.2Q�6�   �� 5k   
   -   5        $& 
AllyHPShow��5k      $
��Left=�6(   $��	41,58,0,0q�6C   $
��HP: ��6V   $
��Wrap=�6b   $	��Top=�6v   
�������$��64��6�   $��LeftToRight=�6�   $��Arial Rounded MT Bold)�6�   $��16��6�   $��8(�6�   �� 5l   
   -   5        $& EnemyHPShow��5l      $
��Left=�6)   $��
671,58,0,0q�6D   $
��HP: ��6X   $
��Wrap=�6d   $	��Top=�6x   
�������$��64��6�   $��LeftToRight=�6�   $��Arial Rounded MT Bold)�6�   $��16��6�   $��8(�6�   5m      5n      