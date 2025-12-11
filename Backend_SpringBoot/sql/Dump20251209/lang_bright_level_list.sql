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
-- Table structure for table `bright_level_list`
--

DROP TABLE IF EXISTS `bright_level_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bright_level_list` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `uid` char(32) NOT NULL COMMENT '关卡32位UID',
  `cover` varchar(255) NOT NULL COMMENT '封面',
  `json` json NOT NULL COMMENT '关卡数据',
  `label` varchar(100) NOT NULL COMMENT '关卡名称locales',
  `describe` text,
  `category_id` int(11) DEFAULT '0' COMMENT '分类编号',
  `default_unlock` tinyint(4) DEFAULT '0' COMMENT '是否默认解锁',
  `max_stars` int(11) DEFAULT '0' COMMENT '最大星星数',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `uid` (`uid`)
) ENGINE=MyISAM AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='关卡信息表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bright_level_list`
--

LOCK TABLES `bright_level_list` WRITE;
/*!40000 ALTER TABLE `bright_level_list` DISABLE KEYS */;
INSERT INTO `bright_level_list` VALUES (1,'D7DF161DF5D03C75468C0B882DA288C1','gameassets/covers/level_forest_advanture.png','{\"AddressableBundle\": \"DLC_TestScene\"}','#FOREST_LABEL_CHAPTERMENU','新手教程',1,1,3,'linehey999','2025-12-06 12:38:58','linehey999','2025-12-10 22:06:12'),(2,'9EA609884AC13C6822ABBD81109A8A56','gameassets/covers/level_desert_examing.png','{\"AddressableBundle\": \"DLC_Desert\"}','#DESERT_LABEL_CHAPTERMENU','沙漠挑战',1,0,3,'linehey999','2025-12-06 12:38:58','linehey999','2025-12-10 20:41:42'),(3,'FCFF480AAEBACF071B1D94D2F0B8C45B','gameassets/covers/level_iceworld_advanture.png','{\"AddressableBundle\": \"DLC_IceWorld\"}','#ICELAND_LABEL_CHAPTERMENU','冰雪世界',2,0,4,'linehey999','2025-12-06 12:38:58','linehey999','2025-12-10 20:41:42'),(4,'8815EA379DA315FFC630BDDAAF4046C7','gameassets/covers/level_lava_field.png','{\"AddressableBundle\": \"DLC_LavaField\"}','#LAVA_LABEL_CHAPTERMENU','火山地带',2,0,4,'linehey999','2025-12-06 12:38:58','linehey999','2025-12-10 20:41:42'),(5,'F67BDF564B289638D396FFF3A8E856F4','gameassets/covers/level_sky_city.png','{\"AddressableBundle\": \"DLC_SkyCity\"}','#SKY_LABEL_CHAPTERMENU','空中迷宫',3,0,5,'linehey999','2025-12-06 12:38:58','linehey999','2025-12-10 20:41:42');
/*!40000 ALTER TABLE `bright_level_list` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-12-11 15:57:15
