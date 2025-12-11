CREATE DATABASE  IF NOT EXISTS `lang` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci */;
USE `lang`;
-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: lang
-- ------------------------------------------------------
-- Server version	5.7.26

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bright_dict_data`
--

DROP TABLE IF EXISTS `bright_dict_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bright_dict_data` (
  `dict_code` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '字典编码ID\r\n',
  `dict_label` varchar(100) DEFAULT '' COMMENT '字典显示值-登录',
  `dict_value` varchar(100) DEFAULT '' COMMENT '字典键值-#LOGIN',
  `dict_type` varchar(100) DEFAULT '' COMMENT '字典类型-language',
  `dict_language` varchar(100) DEFAULT NULL COMMENT '语言区域-zhcn',
  `length_list` varchar(100) DEFAULT NULL COMMENT '字典长度',
  `status` char(1) DEFAULT '0' COMMENT '状态（0正常 1停用）',
  `deptid` bigint(20) DEFAULT NULL COMMENT '部门ID',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`dict_code`) USING BTREE
) ENGINE=MyISAM AUTO_INCREMENT=1998018918446510083 DEFAULT CHARSET=utf8 COMMENT='字典数据表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bright_dict_data`
--

LOCK TABLES `bright_dict_data` WRITE;
/*!40000 ALTER TABLE `bright_dict_data` DISABLE KEYS */;
INSERT INTO `bright_dict_data` VALUES (1,'登录','#LOGIN','language','zh',NULL,'0',NULL,'linehey','2025-12-06 00:02:49','linehey','2025-12-06 00:04:21'),(2,'Login','#LOGIN','language','en',NULL,'0',NULL,'linehey','2025-12-06 00:04:26',NULL,'2025-12-06 00:05:22'),(3,'今日公告','#MOTD_TITLE','language','zh',NULL,'0',NULL,'linehey999','2025-12-06 00:04:26',NULL,'2025-12-06 00:05:22'),(4,'TODAY MOTD','#MOTD_TITLE','language','en',NULL,'0',NULL,'linehey999','2025-12-06 00:04:26',NULL,'2025-12-06 00:05:22'),(5,'欢迎小朋友来玩游戏！','#MOTD_CONTENT','language','zh',NULL,'0',NULL,'linehey','2025-12-06 00:02:49','linehey','2025-12-06 00:04:21'),(6,'Welcome to the game! ','#MOTD_CONTENT','language','en',NULL,'0',NULL,'linehey','2025-12-06 00:04:26',NULL,'2025-12-06 00:05:22'),(7,'ysheepy','#YANG','language','en',NULL,'0',NULL,'wyvernlang','2025-12-06 00:04:26','','2025-12-06 00:05:22'),(8,'吔西匹','#YANG','language','zh',NULL,'0',NULL,'wyvernlang','2025-12-06 00:05:22','','2025-12-06 00:05:22'),(1997692526160678914,'测试','#TEST','language','zh',NULL,'0',NULL,'unity_editor','2025-12-07 23:39:48','','2025-12-07 23:39:48'),(1997680220810936322,'Login','#LOGIN','language','en',NULL,'0',NULL,'wyvern','2025-12-07 22:50:54','','2025-12-07 22:50:54'),(1997680430521942017,'注册','#SIGNUP','language','zh',NULL,'0',NULL,'wyvernlang','2025-12-07 22:51:44','','2025-12-07 22:51:44'),(1997680430521942018,'Signup','#SIGNUP','language','en',NULL,'0',NULL,'wyvernlang','2025-12-07 22:51:44','','2025-12-07 22:51:44'),(1997692527167311873,'Test','#TEST','language','en',NULL,'0',NULL,'unity_editor','2025-12-07 23:39:48','','2025-12-07 23:39:48'),(1997693903876296706,'测试','#TEST','language','zh',NULL,'0',NULL,'unity_editor','2025-12-07 23:45:16','','2025-12-07 23:45:16'),(1997693903943405570,'Test','#TEST','language','en',NULL,'0',NULL,'unity_editor','2025-12-07 23:45:16','','2025-12-07 23:45:16'),(1997696594451009538,'在这里输入你的用户名...','#PLACEHOLDER_USERNAME_INPUTFIELD','language','zh',NULL,'0',NULL,'unity_editor','2025-12-07 23:55:58','','2025-12-07 23:55:58'),(1997696594451009539,'Text your username here...','#PLACEHOLDER_USERNAME_INPUTFIELD','language','en',NULL,'0',NULL,'unity_editor','2025-12-07 23:55:58','','2025-12-07 23:55:58'),(1997696665225695233,'在这里输入你的密码...','#PLACEHOLDER_PASSWORD_INPUTFIELD','language','zh',NULL,'0',NULL,'unity_editor','2025-12-07 23:56:15','','2025-12-07 23:56:15'),(1997696665225695234,'And text your password here...','#PLACEHOLDER_PASSWORD_INPUTFIELD','language','en',NULL,'0',NULL,'unity_editor','2025-12-07 23:56:15','','2025-12-07 23:56:15'),(1997698814525476865,'我同意<color=yellow>一切条款</color>。','#AGREEMENT_TERM_TOGGLE','language','zh',NULL,'0',NULL,'unity_editor','2025-12-08 00:04:47','','2025-12-08 00:04:47'),(1997698814525476866,'I AGREE WITH <color=yellow>ANYOTHER TERMS</color>.','#AGREEMENT_TERM_TOGGLE','language','en',NULL,'0',NULL,'unity_editor','2025-12-08 00:04:47','','2025-12-08 00:06:08'),(1998017815260672002,'开始游戏','#PLAY_BUTTON_CHAPTERMENU','language','zh',NULL,'0',NULL,'unity_editor','2025-12-08 21:12:23','','2025-12-08 21:12:23'),(1998017816577683457,'START GAME','#PLAY_BUTTON_CHAPTERMENU','language','en',NULL,'0',NULL,'unity_editor','2025-12-08 21:12:23','','2025-12-08 21:12:23'),(1998018090331516929,'沙漠试炼','#DESERT_LABEL_CHAPTERMENU','language','zh',NULL,'0',NULL,'unity_editor','2025-12-08 21:13:28','','2025-12-08 21:14:39'),(1998018090331516930,'DESERT EXAMING','#DESERT_LABEL_CHAPTERMENU','language','en',NULL,'0',NULL,'unity_editor','2025-12-08 21:13:28','','2025-12-08 21:14:39'),(1998018208925462529,'森林探险','#FOREST_LABEL_CHAPTERMENU','language','zh',NULL,'0',NULL,'unity_editor','2025-12-08 21:13:57','','2025-12-08 21:13:57'),(1998018208925462530,'FOREST ADVANTURE','#FOREST_LABEL_CHAPTERMENU','language','en',NULL,'0',NULL,'unity_editor','2025-12-08 21:13:57','','2025-12-08 21:13:57'),(1998018501855653890,'冰原冒险','#ICELAND_LABEL_CHAPTERMENU','language','zh',NULL,'0',NULL,'unity_editor','2025-12-08 21:15:06','','2025-12-08 21:15:06'),(1998018501855653891,'ICELAND BATTLE','#ICELAND_LABEL_CHAPTERMENU','language','en',NULL,'0',NULL,'unity_editor','2025-12-08 21:15:06','','2025-12-08 21:15:06'),(1998018690079240194,'火山探索','#LAVA_LABEL_CHAPTERMENU','language','zh',NULL,'0',NULL,'unity_editor','2025-12-08 21:15:51','','2025-12-08 21:15:51'),(1998018690079240195,'EXPLORING LAVA','#LAVA_LABEL_CHAPTERMENU','language','en',NULL,'0',NULL,'unity_editor','2025-12-08 21:15:51','','2025-12-08 21:15:51'),(1998018918446510081,'天空之城','#SKY_LABEL_CHAPTERMENU','language','zh',NULL,'0',NULL,'unity_editor','2025-12-08 21:16:46','','2025-12-08 21:16:46'),(1998018918446510082,'THE DOME','#SKY_LABEL_CHAPTERMENU','language','en',NULL,'0',NULL,'unity_editor','2025-12-08 21:16:46','','2025-12-08 21:16:46');
/*!40000 ALTER TABLE `bright_dict_data` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-12-11 15:57:14
