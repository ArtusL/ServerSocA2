
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//[System.Serializable]
//public class SkillData {
//    public string name;
//    public int level;
//    public SkillData(string _name, int _level){
//        name=_name;
//        level=_level;
//    }
//}
//public class SkillBox : MonoBehaviour
//{
//    [SerializeField]TMP_InputField SkillName;
//    [SerializeField]Slider SkillLevelSlider;
//    [SerializeField]TMP_Text SKillLevelText;

//    public SkillData ReturnClass(){
//        return new SkillData(SkillName.text,(int)SkillLevelSlider.value);
//    }
//    public void SetUI(SkillData sk){
//        SkillName.text=sk.name;
//        SkillLevelSlider.value=sk.level;
//    }
//    public void SliderChangeUpdate(float num){

//        SKillLevelText.text=SkillLevelSlider.value.ToString();
//    }   
//}
