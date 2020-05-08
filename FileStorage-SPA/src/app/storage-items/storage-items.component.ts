import { Component, OnInit } from '@angular/core';
import { ActualItemsService } from '../_services/actual-items.service';
import { StorageItem } from '../_models/storageitem';
import { HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-storage-items',
  templateUrl: './storage-items.component.html',
  styleUrls: ['./storage-items.component.css']
})
export class StorageItemsComponent implements OnInit {
  storageItems: StorageItem[];


  constructor(private actualItemsService: ActualItemsService) { }

  ngOnInit() {
    this.getActualItems();
  }

  getActualItems() {
    this.actualItemsService.getActualFiles()
      .subscribe((storageItems: StorageItem[]) => {
        this.storageItems = storageItems;
      }, error => {
        console.log(error);
      });
  }

  public downloadActualItems(id: string, name: string) {
    this.actualItemsService.downloadActualFile(id).subscribe(
      data => {
        switch (data.type) {
          case HttpEventType.Response:
            const downloadedFile = new Blob([data.body], { type: data.body.type });
            const a = document.createElement('a');
            a.setAttribute('style', 'display:none;');
            document.body.appendChild(a);
            a.download = name;
            a.href = URL.createObjectURL(downloadedFile);
            a.target = '_blank';
            a.click();
            document.body.removeChild(a);
            break;
        }
      },
      error => {
        console.log(error);
      });
  }

}
