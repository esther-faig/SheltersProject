import Icon from './Icon';

interface FeatureTagProps {
  text: string;
}

function FeatureTag(props: FeatureTagProps) {
  return (
    <span className="chip ok">
      <Icon name="check" />
      {props.text}
    </span>
  );
}

export default FeatureTag;
