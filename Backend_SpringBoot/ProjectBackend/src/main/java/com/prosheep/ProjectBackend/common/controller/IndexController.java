package com.prosheep.ProjectBackend.common.controller;


import cn.dev33.satoken.annotation.SaIgnore;
import com.prosheep.ProjectBackend.unityapi.entity.R;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@SaIgnore
@RestController
public class IndexController {
    @GetMapping("/")
    public R index(){
        String _help = "平台后端已加载，请使用前台地址访问。";
        return R.ok(_help);
    }

}

