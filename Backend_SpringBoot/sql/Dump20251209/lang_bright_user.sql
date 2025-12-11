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
-- Table structure for table `bright_user`
--

DROP TABLE IF EXISTS `bright_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bright_user` (
  `user_id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `username` varchar(30) NOT NULL COMMENT '用户账号',
  `nickname` varchar(30) NOT NULL COMMENT '用户昵称',
  `usertype` int(2) DEFAULT '0' COMMENT '用户类型（0系统用户）',
  `deptid` bigint(20) DEFAULT NULL COMMENT '部门ID',
  `phonenumber` varchar(50) DEFAULT '' COMMENT '手机号码',
  `sex` char(1) DEFAULT '0' COMMENT '用户性别（0男 1女）',
  `avatar` varchar(100) DEFAULT '' COMMENT '头像地址',
  `password` varchar(100) DEFAULT '' COMMENT '密码',
  `status` char(1) DEFAULT '0' COMMENT '帐号状态（0正常 1停用）',
  `del_flag` char(1) DEFAULT '0' COMMENT '删除标志（0代表存在 1代表删除）',
  `login_ip` varchar(128) DEFAULT '' COMMENT '最后登录IP',
  `login_date` datetime DEFAULT NULL COMMENT '最后登录时间',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=MyISAM AUTO_INCREMENT=12 DEFAULT CHARSET=utf8 COMMENT='用户信息表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bright_user`
--

LOCK TABLES `bright_user` WRITE;
/*!40000 ALTER TABLE `bright_user` DISABLE KEYS */;
INSERT INTO `bright_user` VALUES (10,'john_smith','大鹏',0,NULL,'13500000005','0','useruploads/avatars/john_smith.png','johnSecure!','0','0','203.0.113.55','2025-12-06 10:00:00','admin01','2025-12-06 12:28:14','admin01','2025-12-06 12:28:14','研发部主管'),(9,'old_user','旧账号',0,NULL,'13600000004','1','useruploads/avatars/old_user.png','oldPass789','0','1','172.16.0.20','2025-11-28 16:45:00','system','2025-12-06 12:28:14','system','2025-12-06 12:28:14','已删除用户'),(8,'test_user','测试账号',0,NULL,'13700000003','0','useruploads/avatars/test_user.png','pass','1','0','127.0.0.1','2025-12-01 08:00:00','system','2025-12-06 12:28:14','system','2025-12-06 12:28:14','停用测试账号'),(7,'jane_doe','小鹿',0,NULL,'13900000002','1','useruploads/avatars/jane_doe.png','password456','0','0','10.0.0.5','2025-12-05 14:15:00','admin01','2025-12-06 12:28:14','admin01','2025-12-06 12:28:14','市场部普通用户'),(6,'admin01','系统管理员',1,10086,'13800000001','2','useruploads/avatars/admin01.png','securePass123','0','0','192.168.1.10','2025-12-06 09:30:00','system','2025-12-06 12:28:14','system','2025-12-06 12:28:14','初始管理员账号');
/*!40000 ALTER TABLE `bright_user` ENABLE KEYS */;
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
