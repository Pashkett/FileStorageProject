import { Component, OnInit, TemplateRef } from '@angular/core';
import { StorageItem } from '../_models/storageitem';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PublicItemsService } from '../_services/public-items.service';
import { HttpEventType } from '@angular/common/http';
import { AuthService } from '../_services/auth.service';
import { Pagination, PaginatedResult } from '../_models/pagination';

@Component({
  selector: 'app-shared-items',
  templateUrl: './shared-items.component.html',
  styleUrls: ['./shared-items.component.css']
})
export class SharedItemsComponent implements OnInit {
  publicItems: StorageItem[];
  modalRef: BsModalRef;
  selectedItem: StorageItem;
  currentUserId: string = null;
  pagination: Pagination;
  currentPage = 1;
  pageSize = 7;
  maxSize = 5;
  itemParams: any = {};

  constructor(private publicItemsService: PublicItemsService,
              private modalService: BsModalService,
              private authService: AuthService) { }

  ngOnInit() {
    this.itemParams.order = 'displayName';
    this.itemParams.direction = 'asc';
    this.itemParams.searchTerm = '';

    this.getPublicItems();
  }

  getPublicItems() {
    this.publicItemsService.getPublicFiles(this.currentPage, this.pageSize, this.itemParams)
    .subscribe((paginatedResult: PaginatedResult<StorageItem[]>) => {
      this.publicItems = paginatedResult.result;
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
    this.getPublicItems();
  }

  getCurrentUserId(): string {
    return this.authService.decodedToken.unique_name;
  }

  openModal(template: TemplateRef<any>, item: StorageItem) {
    this.selectedItem = item;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirmPrivate(): void {
    this.setPrivate(this.selectedItem);
    this.selectedItem = null;
    this.modalRef.hide();
  }

  private setPrivate(item: StorageItem) {
    this.publicItemsService.setFilePrivate(item.id)
      .subscribe(
        result => {
          console.log(result);
          const index: number = this.publicItems.indexOf(item);
          if (index !== -1) {
            this.publicItems.splice(index, 1);
          }
        },
        error => console.log(error)
      );
  }

  public downloadPublicItems(id: string, name: string) {
    this.publicItemsService.downloadPublicFile(id).subscribe(
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

  decline(): void {
    this.modalRef.hide();
  }

}
