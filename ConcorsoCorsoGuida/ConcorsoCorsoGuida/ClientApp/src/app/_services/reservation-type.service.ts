import { Injectable, PipeTransform } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Proxy } from '../_helpers';
import { ReservationType, Room, User } from '../_models';
import { SortColumn, SortDirection } from '../_helpers/sortable.directive';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';

interface SearchResult
{
  types: ReservationType[];
  total: number;
}

interface State
{
  page: number;
  pageSize: number;
  searchTerm: string;
  sortColumn: SortColumn;
  sortDirection: SortDirection;
}


@Injectable({ providedIn: 'root' })
export class ReservationTypeService
{


  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _types$ = new BehaviorSubject<ReservationType[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private _types: ReservationType[] = [];

  private _state: State = {
    page: 1,
    pageSize: 4,
    searchTerm: '',
    sortColumn: '',
    sortDirection: ''
  };

  constructor(private http: HttpClient, private proxy: Proxy)
  {
  }

  getAll(): Observable<ReservationType[]>
  {
    return this.http.get<ReservationType[]>(`${window.location.origin}/reservationtype`);
  }

  getEnabled(): Observable<ReservationType[]>
  {
    return this.http.get<ReservationType[]>(`${window.location.origin}/reservationtype/getEnabled`);
  }

  save(inputReservation: Room): Observable<Room>
  {
    return this.proxy.call<Room>("reservationType", "save", inputReservation);
  }

  delete(inputReservation: Room): Observable<boolean>
  {
    return this.proxy.call<boolean>("reservationType", "delete", inputReservation);
  }


  get types$() { return this._types$.asObservable(); }
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }

  set page(page: number) { this._set({ page }); }
  set pageSize(pageSize: number) { this._set({ pageSize }); }
  set searchTerm(searchTerm: string) { this._set({ searchTerm }); }
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }

  private _set(patch: Partial<State>)
  {
    Object.assign(this._state, patch);
    this._search$.next();
  }

 

}
