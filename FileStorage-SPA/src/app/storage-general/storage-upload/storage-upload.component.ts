import { Component, OnInit, Input } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { AuthService } from 'src/app/_services/auth.service';
import { StorageItem } from 'src/app/_models/storageitem';
import { ActualItemsService } from 'src/app/_services/actual-items.service';

@Component({
  selector: 'app-storage-upload',
  templateUrl: './storage-upload.component.html',
  styleUrls: ['./storage-upload.component.css']
})
export class StorageUploadComponent implements OnInit {

  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = 'https://localhost:5001/api/';

  constructor(private authService: AuthService,
              private actualItemsService: ActualItemsService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  // getActualItems() {
  //   this.actualItemsService.getActualFiles()
  //     .subscribe((storageItems: StorageItem[]) => {
  //       this.storageItems = storageItems;
  //     }, error => {
  //       console.log(error);
  //     });
  // }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'ActualItems/files/',
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
