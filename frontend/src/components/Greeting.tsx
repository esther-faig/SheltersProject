interface GreetingProps {
name: string;
age?:number;
}
function Greeting({name,age}:GreetingProps) {
    return <p>hellow {name}. <br/>You are {age} years old.</p>
}
export default Greeting;