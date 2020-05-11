import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RecycledItemsService {
  baseUrl = 'https://localhost:5001/api/';
  filesUrl = 'RecycledItems/files/';

  constructor(private http: HttpClient) { }

  getRecycledFiles() {
    return this.http.get(this.baseUrl + 'RecycledItems/files');
  }

  restoreFile(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + fileId, null);
  }

  deleteFile(fileId: string) {
    return this.http.delete(this.baseUrl + this.filesUrl + fileId);
  }

}
