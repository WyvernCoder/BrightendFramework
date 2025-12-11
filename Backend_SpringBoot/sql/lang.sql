write a mysql initialize command for me, do not use FOREIGN KEY:
The User Table, named "user": id(increasing primary key), username(unique), usernick, usertype(int, default 0), password, phonenumber, create_dept, create_by, create_time, update_by, update_time, del_flag(char(1) default 0)
The User Data Table, named "user_data": id(reference to UserTable), username(reference to UserTable), unlock_status(json), create_dept, create_by, create_time, update_by, update_time
The Public Table, named "public_data": id(increasing primary key), dict_key(example: #CHAPTER01_TITLE, unique), dict_value_zh(example: 第一章), dict_value_en(example: First Chapter), create_dept, create_by, create_time, update_by, update_time


-- 表bright_dict_data
CREATE TABLE IF NOT EXISTS `bright_dict_data` (
  `dict_code` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '字典编码ID\r\n',
  `dict_label` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '' COMMENT '字典显示值-登录',
  `dict_value` varchar(100) DEFAULT '' COMMENT '字典键值-#LOGIN',
  `dict_type` varchar(100) DEFAULT '' COMMENT '字典类型-language',
  `dict_language` varchar(100) DEFAULT NULL COMMENT '语言区域-zhcn',
  `length_list` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL COMMENT '字典长度',
  `status` char(1) DEFAULT '0' COMMENT '状态（0正常 1停用）',
  `deptid` bigint(20) DEFAULT NULL COMMENT '部门ID',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`dict_code`) USING BTREE
) COMMENT='字典数据表';

INSERT INTO `bright_dict_data` 
	(`dict_label`, `dict_value`, `dict_type`, `dict_language`, `length_list`, `status`, `deptid`, `create_by`, `create_time`, `update_by`, `update_time`
) VALUES
	('登录', '#LOGIN', 'language', 'zh', NULL, '0', NULL, 'linehey', '2025-12-06 00:02:49', 'linehey', '2025-12-06 00:04:21'),
	('Login', '#LOGIN', 'language', 'en', NULL, '0', NULL, 'linehey', '2025-12-06 00:04:26', NULL, '2025-12-06 00:05:22'),
	('今日公告', '#MOTD_TITLE', 'language', 'zh', NULL, '0', NULL, 'linehey999', '2025-12-06 00:04:26', NULL, '2025-12-06 00:05:22'),
	('TODAY MOTD', '#MOTD_TITLE', 'language', 'en', NULL, '0', NULL, 'linehey999', '2025-12-06 00:04:26', NULL, '2025-12-06 00:05:22'),
	('欢迎小朋友来玩游戏！', '#MOTD_CONTENT', 'language', 'zh', NULL, '0', NULL, 'linehey', '2025-12-06 00:02:49', 'linehey', '2025-12-06 00:04:21'),
	('Welcome to the game! ', '#MOTD_CONTENT', 'language', 'en', NULL, '0', NULL, 'linehey', '2025-12-06 00:04:26', NULL, '2025-12-06 00:05:22');

-- 表bright_user
CREATE TABLE IF NOT EXISTS `bright_user` (
  `user_id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `username` varchar(30) NOT NULL UNIQUE COMMENT '用户账号',
  `nickname` varchar(30) NOT NULL COMMENT '用户昵称',
  `usertype` int(2) DEFAULT '0' COMMENT '用户类型（0系统用户）',
  `deptid` bigint(20) DEFAULT NULL COMMENT '部门ID',
  `phonenumber` varchar(50) DEFAULT '' COMMENT '手机号码',
  `sex` char(1) DEFAULT '0' COMMENT '用户性别（0男 1女）',
  `avatar` varchar(100) DEFAULT '' COMMENT '头像地址',
  `password` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT '' COMMENT '密码',
  `status` char(1) DEFAULT '0' COMMENT '帐号状态（0正常 1停用）',
  `del_flag` char(1) DEFAULT '0' COMMENT '删除标志（0代表存在 1代表删除）',
  `login_ip` varchar(128) DEFAULT '' COMMENT '最后登录IP',
  `login_date` datetime DEFAULT NULL COMMENT '最后登录时间',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`user_id`)
)  COMMENT='用户信息表';

INSERT INTO bright_user (
  username, nickname, usertype, deptid, phonenumber, sex, avatar, password, status, del_flag, login_ip, login_date, create_by, update_by, remark
) VALUES
  ('admin01', '系统管理员', 1, 10086, '13800000001', '2', 'useruploads/avatars/admin01.png', 'securePass123', '0', '0', '192.168.1.10', '2025-12-06 09:30:00', 'system', 'system', '初始管理员账号'),
  ('jane_doe', '小鹿', 0, NULL, '13900000002', '1', 'useruploads/avatars/jane_doe.png', 'password456', '0', '0', '10.0.0.5', '2025-12-05 14:15:00', 'admin01', 'admin01', '市场部普通用户'),
  ('test_user', '测试账号', 0, NULL, '13700000003', '0', 'useruploads/avatars/test_user.png', 'pass', '1', '0', '127.0.0.1', '2025-12-01 08:00:00', 'system', 'system', '停用测试账号'),
  ('old_user', '旧账号', 0, NULL, '13600000004', '1', 'useruploads/avatars/old_user.png', 'oldPass789', '0', '1', '172.16.0.20', '2025-11-28 16:45:00', 'system', 'system', '已删除用户'),
  ('john_smith', '大鹏', 0, NULL, '13500000005', '0', 'useruploads/avatars/john_smith.png', 'johnSecure!', '0', '0', '203.0.113.55', '2025-12-06 10:00:00', 'admin01', 'admin01', '研发部主管');



-- 表bright_user_data
CREATE TABLE IF NOT EXISTS `bright_user_data` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `username` varchar(30) NOT NULL UNIQUE,
  `unlock_status` json DEFAULT NULL,
  `deptid` bigint(20) DEFAULT NULL COMMENT '部门ID',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  PRIMARY KEY (`id`),
  KEY `idx_userdata_id` (`id`),
  KEY `idx_userdata_username` (`username`)
) COMMENT='用户课程数据表';

INSERT INTO bright_user_data (
  username, unlock_status, deptid, create_by, update_by
) VALUES
  ('admin01', JSON_OBJECT('level_array', JSON_ARRAY(
									JSON_OBJECT('level_uid','D7DF161DF5D03C75468C0B882DA288C1','stars',2,'unlock',1,'process',1),
									JSON_OBJECT('level_uid','9EA609884AC13C6822ABBD81109A8A56','stars',0,'unlock',1,'process',0.15)
								)
						), 10086, 'linehey999', 'linehey999'),

  ('jane_doe', JSON_OBJECT('level_array', JSON_ARRAY(
									JSON_OBJECT('level_uid','D7DF161DF5D03C75468C0B882DA288C1','stars',0,'unlock',1,'process',0),
									JSON_OBJECT('level_uid','9EA609884AC13C6822ABBD81109A8A56','stars',0,'unlock',0,'process',0)
								)
						), NULL, 'linehey999', 'linehey999'),

  ('test_user', JSON_OBJECT('level_array', JSON_ARRAY(
									JSON_OBJECT('level_uid','D7DF161DF5D03C75468C0B882DA288C1','stars',1,'unlock',1,'process',0.75),
									JSON_OBJECT('level_uid','9EA609884AC13C6822ABBD81109A8A56','stars',0,'unlock',0,'process',0)
								)
						), NULL, 'linehey999', 'linehey999'),

  ('old_user', JSON_OBJECT('level_array', JSON_ARRAY(
									JSON_OBJECT('level_uid','D7DF161DF5D03C75468C0B882DA288C1','stars',2,'unlock',1,'process',1),
									JSON_OBJECT('level_uid','9EA609884AC13C6822ABBD81109A8A56','stars',0,'unlock',1,'process',0.15)
								)
						), NULL, 'linehey999', 'linehey999'),

  ('john_smith', JSON_OBJECT('level_array', JSON_ARRAY(
									JSON_OBJECT('level_uid','D7DF161DF5D03C75468C0B882DA288C1','stars',2,'unlock',1,'process',1),
									JSON_OBJECT('level_uid','9EA609884AC13C6822ABBD81109A8A56','stars',0,'unlock',1,'process',0.15)
								)
						), NULL, 'linehey999', 'linehey999');




{
	"level_array": [
	   {
			"level_uid": "D7DF161DF5D03C75468C0B882DA288C1",
			"stars": 2,
			"unlock": 1,
			"process": 1
		},
		{
			"level_uid": "9EA609884AC13C6822ABBD81109A8A56",
			"stars": 0,
			"unlock": 1,
			"process": 0.15
		}
	]
}










-- 表bright_level_list
CREATE TABLE `bright_level_list` (
	`id` bigint(20) NOT NULL AUTO_INCREMENT,
	`uid` CHAR(32) NOT NULL UNIQUE COMMENT '关卡32位UID',
	`cover` VARCHAR(255) NOT NULL COMMENT '封面' COLLATE 'utf8_general_ci',
	`json` json DEFAULT NULL COMMENT '关卡数据',
	`label` VARCHAR(100) NOT NULL COMMENT '关卡名称locales' COLLATE 'utf8_general_ci',
	`describe` TEXT NULL DEFAULT NULL COLLATE 'utf8_general_ci',
	`category_id` INT(11) NULL DEFAULT '0' COMMENT '分类编号',
	`default_unlock` TINYINT(4) NULL DEFAULT '0' COMMENT '是否默认解锁',
	`max_stars` INT(11) NULL DEFAULT '0' COMMENT '最大星星数',
	`create_by` VARCHAR(64) NULL DEFAULT '' COMMENT '创建者' COLLATE 'utf8_general_ci',
	`create_time` DATETIME NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
	`update_by` VARCHAR(64) NULL DEFAULT '' COMMENT '更新者' COLLATE 'utf8_general_ci',
	`update_time` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
	PRIMARY KEY (`id`) USING BTREE
)COMMENT='关卡信息表';

INSERT INTO bright_level_list (
  uid, cover, `json`, label, `describe`, category_id, default_unlock, max_stars, create_by, update_by
) VALUES
  ('D7DF161DF5D03C75468C0B882DA288C1', 'gameassets/covers/level_forest_advanture.png', JSON_OBJECT(), '森林探险', '新手教程', 1, 1, 3, 'linehey999', 'linehey999'),
  ('9EA609884AC13C6822ABBD81109A8A56', 'gameassets/covers/level_desert_examing.png', JSON_OBJECT(), '沙漠试炼', '沙漠挑战', 1, 0, 3, 'linehey999', 'linehey999'),
  ('FCFF480AAEBACF071B1D94D2F0B8C45B', 'gameassets/covers/level_iceworld_advanture.png', JSON_OBJECT(), '冰原冒险', '冰雪世界', 2, 0, 4, 'linehey999', 'linehey999'),
  ('8815EA379DA315FFC630BDDAAF4046C7', 'gameassets/covers/level_lava_field.png', JSON_OBJECT(), '火山熔岩', '火山地带', 2, 0, 4, 'linehey999', 'linehey999'),
  ('F67BDF564B289638D396FFF3A8E856F4', 'gameassets/covers/level_sky_city.png', JSON_OBJECT(), '天空之城', '空中迷宫', 3, 0, 5, 'linehey999', 'linehey999');