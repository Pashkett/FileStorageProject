import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RecycledItemsService {
  baseUrl = 'https://localhost:5001/api/';

constructor(private http: HttpClient) { }

  getRecycledFiles() {
    return this.http.get(this.baseUrl + 'RecycledItems/files');
  }

}
