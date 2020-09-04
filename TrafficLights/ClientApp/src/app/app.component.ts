import { Component, OnInit } from '@angular/core';
import { DataService } from './data.service';
import { TrafficLight } from './trafficLight/trafficLight';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  providers: [DataService]
})
export class AppComponent implements OnInit {

  product: TrafficLight = new TrafficLight();   // изменяемый товар
  products: TrafficLight[];                // массив товаров
  tableMode: boolean = true;          // табличный режим

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.loadProducts();    // загрузка данных при старте компонента  
  }
  // получаем данные через сервис
  loadProducts() {
    this.dataService.getProducts()
      .subscribe((data: TrafficLight[]) => this.products = data);
  }
  // сохранение данных
  save() {
    if (this.product.id == null) {
      this.dataService.createProduct(this.product)
        .subscribe((data: TrafficLight) => this.products.push(data));
    } else {
      this.dataService.updateProduct(this.product)
        .subscribe(data => this.loadProducts());
    }
    this.cancel();
  }
  editProduct(p: TrafficLight) {
    this.product = p;
  }
  cancel() {
    this.product = new TrafficLight();
    this.tableMode = true;
  }
  delete(p: TrafficLight) {
    this.dataService.deleteProduct(p.id)
      .subscribe(data => this.loadProducts());
  }
  add() {
    this.cancel();
    this.tableMode = false;
  }
}
