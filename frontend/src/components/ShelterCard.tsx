import { Link } from 'react-router-dom';
import FeatureTag from './FeatureTag';

interface ShelterCardProps {
  id: number;
  name: string;
  distance: number;
  minutes: number;
  features: string[];
  isRecommended: boolean;
  delay: number;
}

function ShelterCard(props: ShelterCardProps) {
  return (
    <div
      className={props.isRecommended ? 'shelter-card star' : 'shelter-card'}
      style={{ animationDelay: `${props.delay * 0.15}s` }}
    >
      {props.isRecommended && <div className="star-badge">★ המומלץ ביותר</div>}
      <h3>{props.name}</h3>
      <div className="stats">
        <span>📏 {props.distance} מטר</span>
        <span>⏱ {props.minutes} דקות</span>
      </div>
      <div className="tags">
        {props.features.map((feature) => (
          <FeatureTag key={feature} text={feature} />
        ))}
      </div>
      <Link className="btn" to={`/shelter/${props.id}`}>ניווט ←</Link>
    </div>
  );
}

export default ShelterCard;