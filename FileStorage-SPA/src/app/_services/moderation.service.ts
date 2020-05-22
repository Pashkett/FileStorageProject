import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { StorageItem } from '../_models/storageitem';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ModerationService {
  baseUrl = 'https://localhost:5001/api/';
  filesUrl = 'moderation/files/';
  publicActionUrl = 'public/';
  privateActionUrl = 'private/';
  recycleActionUrl = 'recycle/';
  restoreActionUrl = 'restore/';

  constructor(private http: HttpClient) { }

  getFilesForModeration(page?, itemsPerPage?, itemParams?): Observable<PaginatedResult<StorageItem[]>> {
    const paginatedResult: PaginatedResult<StorageItem[]> = new PaginatedResult<StorageItem[]>();

    let params = new HttpParams();

    if (page != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (itemParams != null) {

      if (itemParams?.order) {
        const orderBy = itemParams.order.concat(' ', itemParams?.direction);
        params = params.append('orderBy', orderBy);
      }

      if (itemParams?.searchTerm) {
        params = params.append('searchTerm', itemParams.searchTerm);
      }
    }

    return this.http.get<StorageItem[]>(this.baseUrl + this.filesUrl, { observe: 'response', params })
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

  moveToRecycleBin(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + this.recycleActionUrl + fileId, null);
  }

  moveToPublic(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + this.publicActionUrl + fileId, null);
  }

  moveToPrivate(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + this.privateActionUrl + fileId, null);
  }

  restoreFile(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + this.restoreActionUrl + fileId, null);
  }

  deleteFile(fileId: string) {
    return this.http.delete(this.baseUrl + this.filesUrl + fileId);
  }

}
