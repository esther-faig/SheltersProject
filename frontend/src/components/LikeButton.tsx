import {useState} from 'react';
function LikeButton() {
    const [liked,setLiked]=useState(false);
    const [likeCount,setLikeCount]=useState(0);
    return (
    <div>
    <button onClick={() => {setLiked(!liked);setLikeCount(likeCount + 1)}} >{liked? "Unlike": "Like"}
</button>
<br/>
<span>{likeCount}</span>
    </div>
    );
}
export default LikeButton;