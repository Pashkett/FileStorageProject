import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpRequest, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { StorageItem } from '../_models/storageitem';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PublicItemsService {
  baseUrl = 'https://localhost:5001/api/';
  filesUrl = 'PublicItems/files/';

  constructor(private http: HttpClient) { }

  getPublicFiles(page = 1, itemsPerPage = 8): Observable<PaginatedResult<StorageItem[]>> {
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

  setFilePrivate(fileId: string) {
    return this.http.post(this.baseUrl + this.filesUrl + fileId, null);
  }

  downloadPublicFile(fileId: string): Observable<HttpEvent<Blob>> {
    return this.http.request(new HttpRequest(
      'GET',
      `${this.baseUrl}${this.filesUrl}${fileId}`,
      null,
      {
        responseType: 'blob'
      }));
  }

}
