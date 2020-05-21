import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpEvent, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StorageItem } from '../_models/storageitem';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class ActualItemsService {
  baseUrl = 'https://localhost:5001/api/';
  filesUrl = 'privateItems/files/';

  constructor(private http: HttpClient) { }

  getActualFiles(page?, itemsPerPage?, itemParams?): Observable<PaginatedResult<StorageItem[]>> {
    const paginatedResult: PaginatedResult<StorageItem[]> = new PaginatedResult<StorageItem[]>();

    let params = new HttpParams();

    if (page != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if (itemParams != null) {
      params = params.append('minSize', (itemParams.minSize * 1024 * 1024).toString());
      params = params.append('maxSize', (itemParams.maxSize * 1024 * 1024).toString());

      if (itemParams?.order) {
        const orderBy = itemParams.order.concat(' ', itemParams?.direction);
        params = params.append('orderBy', orderBy);
      }

      if (itemParams?.searchTerm) {
        params = params.append('searchTerm', itemParams.searchTerm);
      }
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

  downloadActualFile(fileId: string): Observable<HttpEvent<Blob>> {
    return this.http.request(new HttpRequest(
      'GET',
      `${this.baseUrl}${this.filesUrl}${fileId}`,
      null,
      {
        responseType: 'blob'
      }));
  }

  moveToRecycleBin(fileId: string) {
    return this.http.delete(this.baseUrl + this.filesUrl + fileId);
  }

  moveToPublic(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + fileId, null);
  }
}
