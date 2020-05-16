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
  filesUrl = 'ActualItems/files/';

  constructor(private http: HttpClient) { }

  getActualFiles(page = 1, itemsPerPage = 8): Observable<PaginatedResult<StorageItem[]>> {
    const paginatedResult: PaginatedResult<StorageItem[]> = new PaginatedResult<StorageItem[]>();

    let params = new HttpParams();

    if (page != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http.get<StorageItem[]>(this.baseUrl + 'ActualItems/files/', {observe: 'response', params})
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
