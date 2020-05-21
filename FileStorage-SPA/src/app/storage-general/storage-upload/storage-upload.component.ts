import { Component, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-storage-upload',
  templateUrl: './storage-upload.component.html',
  styleUrls: ['./storage-upload.component.css']
})
export class StorageUploadComponent implements OnInit {

  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = 'https://localhost:5001/api/';

  constructor() { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'privateItems/files/',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 500 * 1024 * 1024
    });

    this.uploader.onBeforeUploadItem = (file) => {
      file.withCredentials = false;
    };

  }

}
