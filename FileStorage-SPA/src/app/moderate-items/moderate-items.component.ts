import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StorageItem } from '../_models/storageitem';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { ModerationService } from '../_services/moderation.service';

@Component({
  selector: 'app-moderate-items',
  templateUrl: './moderate-items.component.html',
  styleUrls: ['./moderate-items.component.css']
})
export class ModerateItemsComponent implements OnInit {
  storageItems: StorageItem[];
  modalRef: BsModalRef;
  selectedItem: StorageItem;
  pagination: Pagination;
  currentPage = 1;
  pageSize = 7;
  maxSize = 5;
  isCollapsed = true;
  itemParams: any = {};

  constructor(private moderationService: ModerationService,
              private modalService: BsModalService) { }

  ngOnInit() {
    this.itemParams.order = 'displayName';
    this.itemParams.direction = 'asc';
    this.itemParams.searchTerm = '';

    this.getAllItems();
  }

  getAllItems() {
    this.moderationService.getFilesForModeration(this.currentPage, this.pageSize, this.itemParams)
    .subscribe((paginatedResult: PaginatedResult<StorageItem[]>) => {
      this.storageItems = paginatedResult.result;
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
    this.getAllItems();
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
    this.moderationService.restoreFile(item.id).subscribe(
      result => {
        item.isRecycled = false;
      },
        error => console.log(error)
      );
  }

  confirmPrePublic(): void {
    this.moveToPublic(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  private moveToPublic(item: StorageItem) {
    this.moderationService.moveToPublic(item.id).subscribe(
      result => {
        item.isPublic = true;
      },
      error => console.log(error)
    );
  }

  confirmPrivate(): void {
    this.setPrivate(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  private setPrivate(item: StorageItem) {
    this.moderationService.moveToPrivate(item.id)
      .subscribe(
        result => {
          item.isPublic = false;
        },
        error => console.log(error)
      );
  }

  confirmRecycle(): void {
    this.moveToRecycleBin(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  private moveToRecycleBin(item: StorageItem) {
    this.moderationService.moveToRecycleBin(item.id).subscribe(
      result => {
        item.isRecycled = true;
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
    this.moderationService.deleteFile(item.id).subscribe(
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

  decline(): void {
    this.modalRef.hide();
  }

}
