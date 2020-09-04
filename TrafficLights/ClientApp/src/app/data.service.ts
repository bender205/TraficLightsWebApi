import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TrafficLight } from './trafficLight/trafficLight';

@Injectable()
export class DataService {

  private url = "/api/products";

  constructor(private http: HttpClient) {
  }

  getProducts() {
    return this.http.get(this.url);
  }

  getProduct(id: number) {
    return this.http.get(this.url + '/' + id);
  }

  createProduct(product: TrafficLight) {
    return this.http.post(this.url, product);
  }
  updateProduct(product: TrafficLight) {

    return this.http.put(this.url, product);
  }
  deleteProduct(id: number) {
    return this.http.delete(this.url + '/' + id);
  }
}
