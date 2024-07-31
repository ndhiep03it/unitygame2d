using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character
{
    public int Quyen = 0;
    public int Intro = 0;
    public int CharacterID = 0;
    public int Tuchoiketban = 0;
    public int Diemlike = 0;
    public string lichsutrom ="Trống";
    public int sohatdauthan = 0;
    public int leveldauthan = 1;
    public int status;
    public int Likes = 0;
    public int level = 1;
    public int Dame = 1;
    public float LevelCount = 1;
    public int Hp = 100;
    public int HpMax = 100;
    public int Mp = 100;
    public int Map = 0;
    public float Xpos = 0;
    public float Ypos = 0;
    public int gold = 1;
    public int diamond = 1;

    public Character(int level, float Xpos, float Ypos, int Map, float LevelCount, int Hp, int Mp, int Dame, int HpMax, int gold, int diamond,int Quyen,
        int CharacterID,int Intro,int Likes,int status, int Tuchoiketban, int Diemlike,int sohatdauthan, int leveldauthan,string lichsutrom)
    {

        this.Quyen = Quyen;
        this.lichsutrom = lichsutrom;
        this.CharacterID = CharacterID;
        this.Tuchoiketban = Tuchoiketban;
        this.Diemlike = Diemlike;
        this.sohatdauthan = sohatdauthan;
        this.leveldauthan = leveldauthan;
        this.status = status;
        this.Likes = Likes;
        this.Intro = Intro;
        this.level = level;
        this.Map = Map;
        this.Xpos = Xpos;
        this.Ypos = Ypos;
        this.LevelCount = LevelCount;
        this.Hp = Hp;
        this.HpMax = HpMax;
        this.Mp = Mp;
        this.Dame = Dame;
        this.gold = gold;
        this.diamond = diamond;

    }

}
public class PlayerData : MonoBehaviour
{
    public static PlayerData Singleton;
    public  int CharacterID = 0;
    public string lichsutrom = "Trống";
    public int Tuchoiketban = 0;
    public  int Diemlike = 0;
    public  int sohatdauthan = 0;
    public  int leveldauthan = 0;
    public  int status;
    public  int Likes = 0;
    public  int Intro = 0;
    public  int Quyen = 0;
    public  int level = 1;
    public  int Dame = 10;
    public  int Hp = 100;
    public  int HpMax = 100;
    public  int Mp = 100;
    public  float LevelCount = 1;
    public  int Map = 1;
    public  float Xpos = 0;
    public  float Ypos = 0;
    public  int gold = 1;
    public  int diamond = 1;
    public Character ReturnClass()
    {
        return new Character(level, Xpos, Ypos, Map, LevelCount, Hp, Mp, Dame, HpMax, gold, diamond,
        Quyen,CharacterID,Intro,Likes,status,Tuchoiketban,Diemlike,sohatdauthan,leveldauthan,lichsutrom);
    }

    public void SetDataUI(Character character)
    {
        CharacterID = character.CharacterID;
        Tuchoiketban = character.Tuchoiketban;
        Diemlike = character.Diemlike;
        //sohatdauthan = character.sohatdauthan;
        leveldauthan = character.leveldauthan;
        status = character.status;
        Likes = character.Likes;
        Intro = character.Intro;
        level = character.level;
        Quyen = character.Quyen;
        Xpos = character.Xpos;
        Ypos = character.Ypos;
        Map = character.Map;
        LevelCount = character.LevelCount;
        Hp = character.Hp;
        HpMax = character.HpMax;
        Mp = character.Mp;
        Dame = character.Dame;
        gold = character.gold;
        diamond = character.diamond;
    }
    public void SetLichSuUI(Character character)
    {
      
        lichsutrom = character.lichsutrom;
       
    }
    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;
            //DontDestroyOnLoad(gameObject); // sẽ tạo cái mới,không tự hủy khi chuyển giữa các scene
        }
        else { }

    }


    public void UpLevel(float Levelcount)
    {
        LevelCount += Levelcount;
        if (LevelCount >= 100)
        {
            LevelCount = 0;
            level += 1;
        }
    }

}
