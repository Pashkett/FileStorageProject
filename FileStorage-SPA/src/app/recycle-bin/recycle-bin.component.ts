import { Component, OnInit, TemplateRef } from '@angular/core';
import { RecycledItemsService } from '../_services/recycled-items.service';
import { StorageItem } from '../_models/storageitem';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-recycle-bin',
  templateUrl: './recycle-bin.component.html',
  styleUrls: ['./recycle-bin.component.css']
})
export class RecycleBinComponent implements OnInit {
  recycledItems: StorageItem[];
  modalRef: BsModalRef;
  selectedItem: StorageItem;

  constructor(private recycledItemsService: RecycledItemsService,
              private modalService: BsModalService) { }

  ngOnInit() {
    this.getAllRecycledItems();
  }

  getAllRecycledItems() {
    this.recycledItemsService.getRecycledFiles()
      .subscribe((recycledItems: StorageItem[]) => {
        this.recycledItems = recycledItems;
      }, error => {
        console.log(error);
      });
  }

  openModal(template: TemplateRef<any>, item: StorageItem) {
    this.selectedItem = item;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirmRestore(): void {
    this.restoreItem(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  private restoreItem(item: StorageItem) {
    this.recycledItemsService.restoreFile(item.id)
      .subscribe((id: string) => {
        console.log(id);
        const index: number = this.recycledItems.indexOf(item);
        if (index !== -1) {
          this.recycledItems.splice(index, 1);
        }
      },
        error => console.log(error)
      );
  }

  confirmDelete(): void {
    this.deleteItem(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  deleteItem(item: StorageItem) {
    this.recycledItemsService.deleteFile(item.id).subscribe(
      result => {
        console.log(result);
        const index: number = this.recycledItems.indexOf(item);
        if (index !== -1) {
          this.recycledItems.splice(index, 1);
        }
      },
      error => console.log(error)
    );
  }

  decline(): void {
    this.modalRef.hide();
  }

}
