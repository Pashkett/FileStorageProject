import { Component, OnInit } from '@angular/core';
import { RecycledItemsService } from '../_services/recycled-items.service';
import { StorageItem } from '../_models/storageitem';


@Component({
  selector: 'app-recycle-bin',
  templateUrl: './recycle-bin.component.html',
  styleUrls: ['./recycle-bin.component.css']
})
export class RecycleBinComponent implements OnInit {
  recycledItems: StorageItem[];

  constructor(private recycledItemsService: RecycledItemsService) { }

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
}
