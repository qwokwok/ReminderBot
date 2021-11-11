using System;
using System.Collections.Generic;
using System.Text;

namespace DontForgetYourHW
{
    static class Constants
    {
        /// <summary>
        /// Genshin Impact for the Deaf
        /// </summary>
        public const long GIFORDEAFSERVER = 764948882056282112;
        public const long GIFORDEAFCHANNEL = 785398582316498985;

        /// <summary>
        /// My own server for testing
        /// </summary>
        public const long MYSERVER = 743004274232918047;
        public const long MYCHANNEL = 772358312297496656;

        /// <summary>
        /// Dakkea's server [Voidity]
        /// </summary>
        public const long VOIDSERVER = 392553371074166787;
        public const long VOIDCHANNEL = 753349368404246668;

        //----------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Element colors
        /// </summary>
        public static Discord.Color ELECTRO = new Discord.Color(146, 80, 177);
        public static Discord.Color ANEMO = new Discord.Color(97, 212, 173);
        public static Discord.Color CYRO = new Discord.Color(172, 246, 252);
        public static Discord.Color PYRO = new Discord.Color(255, 93, 67);
        public static Discord.Color HYDRO = new Discord.Color(0, 200, 238);
        public static Discord.Color GEO = new Discord.Color(255, 221, 128);

        //----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Razor
        /// </summary>
        public const string RAZORICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787565021622173696/razor.png";
        public const string RAZORIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787484812034048060/razor.jpg";

        /// <summary>
        /// Lisa
        /// </summary>
        public const string LISAICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741631977226291/lisa.png";
        public const string LISAIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Beidou
        /// </summary>
        public const string BEIDOUICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741674083713034/beidou.png";
        public const string BEIDOUIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756785196466186/beidou.png";

        /// <summary>
        /// Fischl
        /// </summary>
        public const string FISCHLICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741689544310834/fischl.png";
        public const string FISCHLIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787753988598661140/fischl.png";

        /// <summary>
        /// Keqing
        /// </summary>
        public const string KEQINGICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741710423293962/keqing.png";
        public const string KEQINGIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754047717244939/keqing.png";

        /// <summary>
        /// Jean
        /// </summary>
        public const string JEANICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741699279421522/jean.png";
        public const string JEANIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754026627235900/jean.png";

        /// <summary>
        /// Sucrose
        /// </summary>
        public const string SUCROSEICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741644174786600/sucrose.png";
        public const string SUCROSEIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754016633258004/sucrose.png";

        /// <summary>
        /// Venti
        /// </summary>
        public const string VENTIICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741645126762496/venti.png";
        public const string VENTIIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787753994960240672/venti.png";

        /// <summary>
        /// Xiao
        /// </summary>
        public const string XIAOICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741648661905498/xiao.png";
        public const string XIAOIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Zhongli
        /// </summary>
        public const string ZHONGLIICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787744903483883530/zhongli.png";
        public const string ZHONGLIIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754005128413224/zhongli.png";

        /// <summary>
        /// Noelle
        /// </summary>
        public const string NOELLEICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741637760778240/noelle.png";
        public const string NOELLEIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Ningguang
        /// </summary>
        public const string NINGICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741635135799326/ningguang.png";
        public const string NINGIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754037969420318/ning.png";

        /// <summary>
        /// Amber
        /// </summary>
        public const string AMBERICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787744986715914271/amber.png";
        public const string AMBERIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Bennett
        /// </summary>
        public const string BENNETTICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787744976750510090/bennett.png";
        public const string BENNETIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754042436222986/bennett.png";

        /// <summary>
        /// Xinyan
        /// </summary>
        public const string XINYANICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787744999550484510/xinyan.png";
        public const string XINYANIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Diluc
        /// </summary>
        public const string DILUCICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741684745240576/diluc.png";
        public const string DILUCIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754032969154650/diluc.png";

        /// <summary>
        /// Klee
        /// </summary>
        public const string KLEEICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741566130192434/klee.png";
        public const string KLEEIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754054184861736/klee.png";

        /// <summary>
        /// Xiangling
        /// </summary>
        public const string XIANGLINGICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741646816935976/xiangling.png";
        public const string XIANGLINGIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787753982751801404/xiangling.png";

        /// <summary>
        /// Chongyun
        /// </summary>
        public const string CHONGYUNICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741680442015764/chongyun.png";
        public const string CHONGYUNIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Kaeya
        /// </summary>
        public const string KAEYAICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741704760590386/kaeya.png";
        public const string KAEYAIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Qiqi
        /// </summary>
        public const string QIQIICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741640105918534/qiqi.png";
        public const string QIQIIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Diona
        /// </summary>
        public const string DIONAICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741568928448572/diona.png";
        public const string DIONAIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787753999922233374/diona.png";

        /// <summary>
        /// Ganyu
        /// </summary>
        public const string GANYUICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741694816419861/ganyu.png";
        public const string GANYUIMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Xingqiu
        /// </summary>
        public const string XINGQIUICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787745128961146880/xingqiu.png";
        public const string XINGQIUMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754011259437106/xingqiu.png";

        /// <summary>
        /// Mona
        /// </summary>
        public const string MONAICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741634116190228/mona.png";
        public const string MONAMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";

        /// <summary>
        /// Childe
        /// </summary>
        public const string CHILDEICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787741676533186570/childe.png";
        public const string CHILDEMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787754022009831454/childe.png";

        /// <summary>
        /// Barbara
        /// </summary>
        public const string BARBARAICONURL = "https://cdn.discordapp.com/attachments/787484790302441482/787745119993462804/barbara.png";
        public const string BARBARAMAGEURL = "https://cdn.discordapp.com/attachments/787484790302441482/787756089470222336/blank.png";
    }
}
