package com.prosheep.ProjectBackend.unityapi.service;

import java.io.FileNotFoundException;
import java.net.MalformedURLException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;

import org.springframework.core.io.Resource;
import org.springframework.core.io.UrlResource;
import org.springframework.stereotype.Service;

import com.prosheep.ProjectBackend.unityapi.entity.R;

@Service
public class FileService {

    /**
     * 保存字节文件到本地
     * 
     * @param body     字节流
     * @param filename 文件名及后缀
     * @return 响应实体及文件路径
     */
    public static R<String> FileSaveToLocal(byte[] body, String filename) {

        // Security: remove any path (prevent directory traversal)
        filename = new java.io.File(filename).getName();

        // Construct Save Path
        var saveRelativePath = Paths.get("useruploads/" + filename);

        // Try to save raw-binary file
        Path saveFullpath;
        try {
            Files.createDirectories(saveRelativePath.getParent());
            saveFullpath = Files.write(saveRelativePath, body, StandardOpenOption.CREATE,
                    StandardOpenOption.TRUNCATE_EXISTING);
            System.out.println("Trying save to " + saveFullpath.toAbsolutePath());
        } catch (java.io.IOException e) {
            // e.printStackTrace();
            System.out.println(e.getMessage());
            return R.fail("Failed to upload due to a server error! ", "");
        }

        return R.ok("Upload Succeed! ", saveRelativePath.toString());
    }

    /**
     * 从服务器加载文件
     * 
     * @param filename      文件名及后缀
     * @param useUserUpload 是否从用户上传目录加载
     * @return 响应实体及资源
     * @throws FileNotFoundException 
     * @throws MalformedURLException 
     */
    public static R<Resource> LoadFileOnServer(String filename, Boolean useUserUpload) throws FileNotFoundException, MalformedURLException {
        Path path;
        if (useUserUpload) {
            path = Paths.get("useruploads/" + filename);
        } else {
            path = Paths.get("gameassets/" + filename);
        }

        if (Files.exists(path) == false) {
            System.out.println("File not found on server: " + path.toAbsolutePath());
            throw new java.io.FileNotFoundException("File not found on server. ");
        } else {
            return R.ok("Operation Successed. ", new UrlResource(path.toUri()));
        }
    }

    /**
     * 从服务器加载文件
     *
     * @param filename      文件名及后缀
     * @return 响应实体及资源
     * @throws FileNotFoundException
     * @throws MalformedURLException
     */
    public static R<Resource> LoadDLCFileOnServer(String filename) throws FileNotFoundException, MalformedURLException {
        Path path;
        path = Paths.get("gameassets/dlcs/" + filename);

        if (Files.exists(path) == false) {
            System.out.println("File not found on server: " + path.toAbsolutePath());
            throw new java.io.FileNotFoundException("File not found on server. ");
        } else {
            return R.ok("Operation Successed. ", new UrlResource(path.toUri()));
        }
    }


}
