import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActualItemsService } from '../../_services/actual-items.service';
import { StorageItem } from '../../_models/storageitem';
import { HttpEventType } from '@angular/common/http';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-storage-items',
  templateUrl: './storage-items.component.html',
  styleUrls: ['./storage-items.component.css']
})
export class StorageItemsComponent implements OnInit {
  storageItems: StorageItem[];
  modalRef: BsModalRef;
  selectedItem: StorageItem;

  constructor(private actualItemsService: ActualItemsService,
              private modalService: BsModalService) { }

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

  openModal(template: TemplateRef<any>, item: StorageItem) {
    this.selectedItem = item;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirmPreDelete(): void {
    this.moveToRecycleBin(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  confirmPrePublic(): void {
    this.moveToPublic(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  decline(): void {
    this.modalRef.hide();
  }

  downloadActualItems(id: string, name: string) {
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

  private moveToRecycleBin(item: StorageItem) {
    this.actualItemsService.moveToRecycleBin(item.id).subscribe(
      result => {
        console.log(result);
        const index: number = this.storageItems.indexOf(item);
        if (index !== -1) {
          this.storageItems.splice(index, 1);
        }
      },
      error => console.log(error)
    );
  }

  private moveToPublic(item: StorageItem) {
    this.actualItemsService.moveToPublic(item.id).subscribe(
      result => {
        console.log(result);
        item.isPublic = true;
      },
      error => console.log(error)
    );
  }

}
