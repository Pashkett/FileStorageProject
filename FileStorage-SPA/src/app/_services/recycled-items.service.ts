import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { StorageItem } from '../_models/storageitem';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecycledItemsService {
  baseUrl = 'https://localhost:5001/api/';
  filesUrl = 'RecycledItems/files/';

  constructor(private http: HttpClient) { }

  // getRecycledFiles() {
  //   return this.http.get(this.baseUrl + this.filesUrl);
  // }

  getRecycledFiles(page = 1, itemsPerPage = 8): Observable<PaginatedResult<StorageItem[]>> {
    const paginatedResult: PaginatedResult<StorageItem[]> = new PaginatedResult<StorageItem[]>();

    let params = new HttpParams();

    if (page != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http.get<StorageItem[]>(this.baseUrl + this.filesUrl, {observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );

  }

  restoreFile(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + fileId, null);
  }

  deleteFile(fileId: string) {
    return this.http.delete(this.baseUrl + this.filesUrl + fileId);
  }

}
