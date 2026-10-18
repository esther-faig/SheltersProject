interface ProductProps {
title: string;
price: number;
discount?: number;
}
function Product({ title, price, discount }: ProductProps) {
return <p>{title.toUpperCase()}: {price} {discount && `- ${discount}`}</p>;}
// 1.
// price.toUpperCase()
// ההמרה לא מתאימה לטיפוס שהוא מספר
// פה יש מספר אבל צריך להיות מחרוזת
// התיקון
// return <p>{title.toUpperCase()}: {price}</p>;


const total: number = 100;
// 2.
// const total: number = '100';
// המשתנה הוא מספרי אבל מוכנס בו מחרוזת
// התיקון
// const total: number = 100;

function App() {
return <Product title="Book" price={10} />;
// 3.
// <Product title="Book" />
// הקומפוננטה מצפה לקבל 2 פרמטרים אבל בפועל מקבלת רק 1
// התיקון
// return <Product title="Book" price={10} />;
}