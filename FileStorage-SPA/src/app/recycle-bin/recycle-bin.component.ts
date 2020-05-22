import { Component, OnInit, TemplateRef } from '@angular/core';
import { RecycledItemsService } from '../_services/recycled-items.service';
import { StorageItem } from '../_models/storageitem';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Pagination, PaginatedResult } from '../_models/pagination';


@Component({
  selector: 'app-recycle-bin',
  templateUrl: './recycle-bin.component.html',
  styleUrls: ['./recycle-bin.component.css']
})
export class RecycleBinComponent implements OnInit {
  recycledItems: StorageItem[];
  modalRef: BsModalRef;
  selectedItem: StorageItem;
  pagination: Pagination;
  currentPage = 1;
  pageSize = 7;
  maxSize = 5;
  itemParams: any = {};

  constructor(private recycledItemsService: RecycledItemsService,
              private modalService: BsModalService) { }

  ngOnInit() {
    this.itemParams.order = 'displayName';
    this.itemParams.direction = 'asc';
    this.itemParams.searchTerm = '';

    this.getAllRecycledItems();
  }

  getAllRecycledItems() {
    this.recycledItemsService.getRecycledFiles(this.currentPage, this.pageSize, this.itemParams)
    .subscribe((paginatedResult: PaginatedResult<StorageItem[]>) => {
      this.recycledItems = paginatedResult.result;
      if (paginatedResult?.pagination != null) {
        this.pagination = paginatedResult.pagination;
      }
    }, error => {
      console.log(error);
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.currentPage = event.page;
    this.getAllRecycledItems();
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

  private deleteItem(item: StorageItem) {
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
