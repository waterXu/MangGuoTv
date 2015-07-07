using MangGuoTv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MangGuoTv.ViewModels
{
    public abstract class DataTemplateSelector:ContentControl
      {
              //根据newContent的属性，返回所需的DataTemplate
              public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
              {
                  return null;
              }
  
              protected override void OnContentChanged(object oldContent, object newContent)
             {
                 base.OnContentChanged(oldContent, newContent);
                 //根据newContent的属性，选择对应的DataTemplate
                 ContentTemplate = SelectTemplate(newContent, this);
             }
 
     }
    public class ChannelVideoTemplateSelector : DataTemplateSelector
     {
         public DataTemplate BannerImage { get; set; }
         public DataTemplate LandscapeImage { get; set; }
         public DataTemplate NorLandscapeImage { get; set; }
         public DataTemplate TitleTemplate { get; set; }
         public DataTemplate RankTemplate { get; set; }
         public DataTemplate LiveImage { get; set; }
         public DataTemplate AvatorImage { get; set; }
 
         //根据newContent的属性，返回所需的DataTemplate
         public override DataTemplate SelectTemplate(object item, DependencyObject container)
         {
             VideoViewModel model = item as VideoViewModel;
             DataTemplate dt;
             if (model != null)
             {
                 switch (model.type)
                 {
                    
                     case "banner":
                         dt = BannerImage;
                         break;
                     //case "normalAvatorText":
                     //    dt = AvatorImage;
                     //    break;
                     case "largeLandScapeNodesc":
                     case "largeLandScape":
                     case "normalLandScapeNodesc":
                     case "aceSeason":
                         dt = LandscapeImage;
                         break;
                     case "normalLandScape":
                     case "normalAvatorText":
                     case "roundAvatorText":
                     case "tvPortrait":
                     case "moviePortrait":
                     case "live":
                         dt = NorLandscapeImage;
                         break;
                     case "title":
                         dt = TitleTemplate;
                         break;
                     case "rankList":
                         dt = RankTemplate;
                         break;
                     //case "live":
                     //    dt = LiveImage;
                     //    break;
                     //case "unknowModType1":
                     //    //CreateNorLandscapeImages(channeldetail.templateData);
                     //    break;
                     //case "unknowModType2":
                     //    //CreateNorLandscapeImages(channeldetail.templateData);
                     //    break;
                     default:
                         dt = NorLandscapeImage;
                         break;
                 }
                 return dt;
             }
             return base.SelectTemplate(item, container);
         }
     }
}
