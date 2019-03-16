# Manager

### Windows Presentation Fundation
**Data bindig**
 - Sample for using data binding with EF entities
 - Using INotifyPropertyChanged with entities

**Awesome Font**

Use Awesome Font icons in WPF.
 1. Download it for desktop [Font Awesome](https://fontawesome.com/download)
 2. In App.xaml put:
  ```axml
   <FontFamily x:Key="fa">/CM;component/Assets/Font/FontAwesome.otf#Font Awesome 5 Free Solid</FontFamily>
  ```
  3. Use it the font like this by putting the  **Unicode Glyph** in the text property:
  ```axml
   <TextBlock Text="" FontFamily="{StaticResource fa}"/>
  ```
