import { Link } from 'react-router-dom';
import FeatureTag from './FeatureTag';
import Icon from './Icon';
import ShelterIllustration from './ShelterIllustration';

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
    <article
      className={props.isRecommended ? 'card reco' : 'card'}
      style={{ animationDelay: `${props.delay * 0.15}s` }}
    >
      {props.isRecommended && (
        <div className="ribbon"><Icon name="star" />המומלץ ביותר</div>
      )}
      <div className="sh-card">
        <div className="thumb"><ShelterIllustration /></div>
        <div className="sh-body">
          <h3>{props.name}</h3>
          <div className="meta">
            <span><Icon name="pin" />{props.distance} מטר</span>
            <span><Icon name="clock" />{props.minutes} דקות</span>
          </div>
          <div className="chips">
            {props.features.map((feature) => (
              <FeatureTag key={feature} text={feature} />
            ))}
          </div>
          <div className="sh-actions">
            <Link className="btn primary small" to={`/shelter/${props.id}`}>
              <span className="sh"><Icon name="nav" /></span>
              ניווט
            </Link>
          </div>
        </div>
      </div>
    </article>
  );
}

export default ShelterCard;
