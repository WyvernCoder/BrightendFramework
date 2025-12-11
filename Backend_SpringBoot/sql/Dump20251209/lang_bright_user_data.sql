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
-- Table structure for table `bright_user_data`
--

DROP TABLE IF EXISTS `bright_user_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bright_user_data` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `username` varchar(30) NOT NULL,
  `unlock_status` json DEFAULT NULL,
  `deptid` bigint(20) DEFAULT NULL COMMENT '部门ID',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`),
  KEY `idx_userdata_id` (`id`),
  KEY `idx_userdata_username` (`username`)
) ENGINE=MyISAM AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='用户课程数据表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bright_user_data`
--

LOCK TABLES `bright_user_data` WRITE;
/*!40000 ALTER TABLE `bright_user_data` DISABLE KEYS */;
INSERT INTO `bright_user_data` VALUES (1,'admin01','{\"level_array\": [{\"stars\": 2, \"unlock\": 1, \"process\": 1, \"level_uid\": \"D7DF161DF5D03C75468C0B882DA288C1\"}, {\"stars\": 0, \"unlock\": 1, \"process\": 0.15, \"level_uid\": \"9EA609884AC13C6822ABBD81109A8A56\"}]}',0,'linehey999','2025-12-06 12:37:06','Rider_new','2025-12-06 16:00:56'),(2,'jane_doe','{\"level_array\": [{\"stars\": 0, \"unlock\": 1, \"process\": 0, \"level_uid\": \"D7DF161DF5D03C75468C0B882DA288C1\"}, {\"stars\": 0, \"unlock\": 0, \"process\": 0, \"level_uid\": \"9EA609884AC13C6822ABBD81109A8A56\"}]}',NULL,'linehey999','2025-12-06 12:37:06','linehey999','2025-12-06 12:37:06'),(3,'test_user','{\"level_array\": [{\"stars\": 1, \"unlock\": 1, \"process\": 0.75, \"level_uid\": \"D7DF161DF5D03C75468C0B882DA288C1\"}, {\"stars\": 0, \"unlock\": 0, \"process\": 0, \"level_uid\": \"9EA609884AC13C6822ABBD81109A8A56\"}]}',NULL,'linehey999','2025-12-06 12:37:06','linehey999','2025-12-06 12:37:06'),(4,'old_user','{\"level_array\": [{\"stars\": 2, \"unlock\": 1, \"process\": 1, \"level_uid\": \"D7DF161DF5D03C75468C0B882DA288C1\"}, {\"stars\": 0, \"unlock\": 1, \"process\": 0.15, \"level_uid\": \"9EA609884AC13C6822ABBD81109A8A56\"}]}',NULL,'linehey999','2025-12-06 12:37:06','linehey999','2025-12-06 12:37:06'),(5,'john_smith','{\"level_array\": [{\"stars\": 2, \"unlock\": 1, \"process\": 1, \"level_uid\": \"D7DF161DF5D03C75468C0B882DA288C1\"}, {\"stars\": 0, \"unlock\": 1, \"process\": 0.15, \"level_uid\": \"9EA609884AC13C6822ABBD81109A8A56\"}]}',NULL,'linehey999','2025-12-06 12:37:06','linehey999','2025-12-06 12:37:06');
/*!40000 ALTER TABLE `bright_user_data` ENABLE KEYS */;
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
