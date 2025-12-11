package com.prosheep.ProjectBackend.unityapi.service;

import com.prosheep.ProjectBackend.unityapi.mapper.UserDataMapper;
import org.springframework.stereotype.Service;

@Service
public class UserDataService {
    private final UserDataMapper mapper;

    public UserDataService(UserDataMapper mapper) {
        this.mapper = mapper;
    }
}
