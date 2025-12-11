package com.prosheep.ProjectBackend.unityapi.controller;

import org.springframework.web.bind.annotation.*;

import com.prosheep.ProjectBackend.unityapi.entity.R;
import com.prosheep.ProjectBackend.unityapi.service.FileService;

import org.springframework.core.io.Resource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.nio.file.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping("v1/files")
public class FileController {

    @PostMapping("upload")
    public R<String> upload(
            @RequestBody byte[] body,
            @RequestHeader("Content-Type") String contentType,
            @RequestHeader("X_FILENAME") String filename) {
        // Check Content-Type
        if (contentType.equalsIgnoreCase("application/octet-stream") == false) {
            System.out.println(String.format("Warning: Forbidden Content-Type request %s detected. ", contentType));
            return R.fail("Forbidden Content-Type! ", contentType);
        }

        return FileService.FileSaveToLocal(body, filename);
    }

    @GetMapping("download_useruploads/{filename}")
    public ResponseEntity<Resource> download_useruploads(@PathVariable String filename) {

        Resource resource;
        try {
            var theFile = FileService.LoadFileOnServer(filename, true);
            resource = theFile.getData();
        } catch (Exception e) {
            System.out.println("Error: File" + filename + " not found on server. ");
            return ResponseEntity.notFound().build();
        }

        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + resource.getFilename() + "\"")
                .contentType(MediaType.APPLICATION_OCTET_STREAM)
                .body(resource);
    }

    @GetMapping("download_gameassets/{filename}")
    public ResponseEntity<Resource> download_gameassets(@PathVariable String filename) {

        Resource resource;
        try {
            var theFile = FileService.LoadFileOnServer(filename, false);
            resource = theFile.getData();
        } catch (Exception e) {
            System.out.println("Error: File" + filename + " not found on server. ");
            return ResponseEntity.notFound().build();
        }

        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + resource.getFilename() + "\"")
                .contentType(MediaType.APPLICATION_OCTET_STREAM)
                .body(resource);
    }

    @GetMapping("download_gameassets/dlcs/{filename}")
    public ResponseEntity<Resource> download_gameassets_dlcs(@PathVariable String filename) {

        Resource resource;
        try {
            var theFile = FileService.LoadDLCFileOnServer(filename);
            resource = theFile.getData();
        } catch (Exception e) {
            System.out.println("Error: File" + filename + " not found on server. ");
            return ResponseEntity.notFound().build();
        }

        return ResponseEntity.ok()
                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + resource.getFilename() + "\"")
                .contentType(MediaType.APPLICATION_OCTET_STREAM)
                .body(resource);
    }

    // ✅ Batch upload
    @PostMapping("upload_gameassets/dlcs/")
    public ResponseEntity<?> uploadDLCFiles(@RequestParam("files") List<MultipartFile> files) {
        List<String> uploaded = new ArrayList<>();
        List<String> failed = new ArrayList<>();

        for (MultipartFile file : files) {
            try {
                Path path = Paths.get("gameassets/dlcs/" + file.getOriginalFilename());
                Files.copy(file.getInputStream(), path, StandardCopyOption.REPLACE_EXISTING);
                uploaded.add(file.getOriginalFilename());
            } catch (IOException e) {
                failed.add(file.getOriginalFilename());
            }
        }

        Map<String, Object> result = new HashMap<>();
        result.put("uploaded", uploaded);
        result.put("failed", failed);

        return ResponseEntity.ok(result);
    }

    // ✅ Batch delete
    @DeleteMapping("delete_gameassets/dlcs/")
    public ResponseEntity<?> deleteDLCFiles(@RequestBody List<String> filenames) {
        List<String> deleted = new ArrayList<>();
        List<String> notFound = new ArrayList<>();

        for (String filename : filenames) {
            Path path = Paths.get("gameassets/dlcs/" + filename);
            try {
                if (Files.exists(path)) {
                    Files.delete(path);
                    deleted.add(filename);
                } else {
                    notFound.add(filename);
                }
            } catch (IOException e) {
                notFound.add(filename);
            }
        }

        Map<String, Object> result = new HashMap<>();
        result.put("deleted", deleted);
        result.put("notFound", notFound);

        return ResponseEntity.ok(result);
    }
}