using System;
using System.Linq;
using NUnit.Framework;
using NoFuture.Util.Pos;

namespace NoFuture.Tests.Util.Pos
{
    [TestFixture]
    public class PennTreebankTagsetTests
    {
        public static string[] AllCodes = new string[]
                                              {
                                                  "CC", "CD", "DT", "EX",
                                                  "FW", "IN", "JJ", "JJR",
                                                  "JJS", "LS", "MD", "NN",
                                                  "NNP", "NNPS", "NNS", "PDT",
                                                  "POS", "PRP", "PRP$", "RB",
                                                  "RBR", "RBS", "RP", "SYM",
                                                  "TO", "UH", "VB", "VBD",
                                                  "VBG", "VBN", "VBP", "VBZ",
                                                  "WDT", "WP", "WP$", "WRB",
                                                  ".", ","
                                              };

        [Test]
        public void TestAllCodesRepresented()
        {
            foreach (var allCode in AllCodes)
            {
                Console.WriteLine(allCode);
                Assert.IsNotNull(NoFuture.Util.Pos.PtTagset.GetTagset(allCode));
            }
        }

        [Test]
        public void TestParseResults()
        {
            const string TEST_INPUT = "This/DT is/VBZ a/DT sample/NN sentence/NN";
            NoFuture.Util.Pos.TagsetBase[] testOutList = null;
            var testResult = NoFuture.Util.Pos.PtTagset.TryParse(TEST_INPUT, out testOutList);

            for (var i = 0; i < testOutList.Length; i++ )
            {
                Console.WriteLine("{0,-8}{1}",i,testOutList[i].Value);
            }

            Assert.IsTrue(testResult);
            Assert.AreEqual(5, testOutList.Count());

            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.Determiner), testOutList[0]);
            Assert.AreEqual("This", testOutList[0].Value);

            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.Verb3rdPersonSingularPresent),testOutList[1]);
            Assert.AreEqual("is", testOutList[1].Value);

            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.Determiner),testOutList[2]);
            Assert.AreEqual("a", testOutList[2].Value);

            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.NounSingularOrMass),testOutList[3]);
            Assert.AreEqual("sample", testOutList[3].Value);

            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.NounSingularOrMass), testOutList[4]);
            Assert.AreEqual("sentence",testOutList[4].Value);
        }

        [Test]
        public void TestSentenceBreakParse()
        {
            NoFuture.Util.Pos.PtTagset.TagDelimiter = '_';
            NoFuture.Util.Pos.TagsetBase[] testOutList;

            var testInput = @"When_WRB in_IN the_DT Course_NNP of_IN human_JJ events_NNS ,_, it_PRP becomes_VBZ necessary_JJ for_IN one_CD people_NNS to_TO dissolve_VB the_DT political_JJ bands_NNS which_WDT have_VBP connected_VBN them_PRP with_IN another_DT ,_, and_CC to_TO assume_VB among_IN the_DT powers_NNS of_IN the_DT earth_NN ,_, the_DT separate_JJ and_CC equal_JJ station_NN to_TO which_WDT the_DT Laws_NNPS of_IN Nature_NNP and_CC of_IN Nature_NNP 's_POS God_NNP entitle_VB them_PRP ,_, a_DT decent_JJ respect_NN to_TO the_DT opinions_NNS of_IN mankind_NN requires_VBZ that_IN they_PRP should_MD declare_VB the_DT causes_NNS which_WDT impel_VBP them_PRP to_TO the_DT separation_NN ._.";
            var testResult = NoFuture.Util.Pos.PtTagset.TryParse(testInput, out testOutList);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testOutList);
            Assert.AreNotEqual(0 ,testOutList.Length);
            Assert.IsTrue(testOutList.Length > 8);
            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.Comma), testOutList[7]);

        }

        [Test]
        public void TestTryParseJaggedArray()
        {
            TagsetBase[][] tagsOut;
            var testInput = @"When_WRB in_IN the_DT Course_NNP of_IN human_JJ events_NNS ,_, it_PRP becomes_VBZ necessary_JJ for_IN one_CD people_NNS to_TO dissolve_VB the_DT political_JJ bands_NNS which_WDT have_VBP connected_VBN them_PRP with_IN another_DT ,_, and_CC to_TO assume_VB among_IN the_DT powers_NNS of_IN the_DT earth_NN ,_, the_DT separate_JJ and_CC equal_JJ station_NN to_TO which_WDT the_DT Laws_NNPS of_IN Nature_NNP and_CC of_IN Nature_NNP 's_POS God_NNP entitle_VB them_PRP ,_, a_DT decent_JJ respect_NN to_TO the_DT opinions_NNS of_IN mankind_NN requires_VBZ that_IN they_PRP should_MD declare_VB the_DT causes_NNS which_WDT impel_VBP them_PRP to_TO the_DT separation_NN ._.";
            var testResult = PtTagset.TryParse(testInput, out tagsOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(tagsOut);
            Assert.AreNotEqual(0, tagsOut.Length);

        }

        [Test]
        public void TestParseResultsBySentences()
        {
            NoFuture.Util.Pos.PtTagset.TagDelimiter = '_';
            NoFuture.Util.Pos.TagsetBase[][] testOutList;
            
            var testResult = NoFuture.Util.Pos.PtTagset.TryParse(_largeTestFile, out testOutList);
            Assert.IsTrue(testResult);

            Assert.IsNotNull(testOutList);
            Assert.AreNotEqual(0, testOutList.Length);

            Assert.IsNotNull(testOutList[0]);
            Assert.AreNotEqual(0,testOutList[0].Length);

            Console.WriteLine("Number of sentences '{0}'", testOutList.Length);

            for (var i = 0; i < testOutList.Length;i++ )
            {
                Console.WriteLine("Sentence '{0}' length is '{1}'",i,testOutList[i].Length);
            }

            Assert.IsNotNull(testOutList[1]);
            Assert.AreNotEqual(0,testOutList[1].Length);

            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.Determiner),testOutList[1][0]);
            Assert.IsInstanceOf(typeof(NoFuture.Util.Pos.Adjective),testOutList[1][1]);

            var secondtolastValue = testOutList[36][0].Value;
            var lastValue = testOutList[36][1].Value;

            Console.WriteLine(secondtolastValue);
            Console.WriteLine(lastValue);
        }

        private string _largeTestFile = @"
IN_IN CONGRESS_NNP ,_, July_NNP 4_CD ,_, 1776_CD ._.
The_DT unanimous_JJ Declaration_NN of_IN the_DT thirteen_CD united_JJ States_NNP of_IN America_NNP ,_, When_WRB in_IN the_DT Course_NNP of_IN human_JJ events_NNS ,_, it_PRP becomes_VBZ necessary_JJ for_IN one_CD people_NNS to_TO dissolve_VB the_DT political_JJ bands_NNS which_WDT have_VBP connected_VBN them_PRP with_IN another_DT ,_, and_CC to_TO assume_VB among_IN the_DT powers_NNS of_IN the_DT earth_NN ,_, the_DT separate_JJ and_CC equal_JJ station_NN to_TO which_WDT the_DT Laws_NNPS of_IN Nature_NNP and_CC of_IN Nature_NNP 's_POS God_NNP entitle_VB them_PRP ,_, a_DT decent_JJ respect_NN to_TO the_DT opinions_NNS of_IN mankind_NN requires_VBZ that_IN they_PRP should_MD declare_VB the_DT causes_NNS which_WDT impel_VBP them_PRP to_TO the_DT separation_NN ._.
We_PRP hold_VBP these_DT truths_NNS to_TO be_VB self-evident_JJ ,_, that_IN all_DT men_NNS are_VBP created_VBN equal_JJ ,_, that_IN they_PRP are_VBP endowed_VBN by_IN their_PRP$ Creator_NNP with_IN certain_JJ unalienable_JJ Rights_NNS ,_, that_IN among_IN these_DT are_VBP Life_NNP ,_, Liberty_NNP and_CC the_DT pursuit_NN of_IN Happiness_NN ._.
--_: That_DT to_TO secure_VB these_DT rights_NNS ,_, Governments_NNS are_VBP instituted_VBN among_IN Men_NNP ,_, deriving_VBG their_PRP$ just_JJ powers_NNS from_IN the_DT consent_NN of_IN the_DT governed_VBN ,_, --_: That_IN whenever_WRB any_DT Form_NN of_IN Government_NNP becomes_VBZ destructive_JJ of_IN these_DT ends_NNS ,_, it_PRP is_VBZ the_DT Right_NNP of_IN the_DT People_NNP to_TO alter_VB or_CC to_TO abolish_VB it_PRP ,_, and_CC to_TO institute_VB new_JJ Government_NNP ,_, laying_VBG its_PRP$ foundation_NN on_IN such_JJ principles_NNS and_CC organizing_VBG its_PRP$ powers_NNS in_IN such_JJ form_NN ,_, as_IN to_TO them_PRP shall_MD seem_VB most_RBS likely_JJ to_TO effect_VB their_PRP$ Safety_NN and_CC Happiness_NN ._.
Prudence_NNP ,_, indeed_RB ,_, will_MD dictate_VB that_IN Governments_NNS long_RB established_VBN should_MD not_RB be_VB changed_VBN for_IN light_JJ and_CC transient_JJ causes_NNS ;_: and_CC accordingly_RB all_DT experience_NN hath_VBP shewn_NN ,_, that_IN mankind_NN are_VBP more_RBR disposed_JJ to_TO suffer_VB ,_, while_IN evils_NNS are_VBP sufferable_JJ ,_, than_IN to_TO right_JJ themselves_PRP by_IN abolishing_VBG the_DT forms_NNS to_TO which_WDT they_PRP are_VBP accustomed_VBN ._.
But_CC when_WRB a_DT long_JJ train_NN of_IN abuses_NNS and_CC usurpations_NNS ,_, pursuing_VBG invariably_RB the_DT same_JJ Object_NNP evinces_VBZ a_DT design_NN to_TO reduce_VB them_PRP under_IN absolute_JJ Despotism_NNP ,_, it_PRP is_VBZ their_PRP$ right_NN ,_, it_PRP is_VBZ their_PRP$ duty_NN ,_, to_TO throw_VB off_RP such_JJ Government_NNP ,_, and_CC to_TO provide_VB new_JJ Guards_NNS for_IN their_PRP$ future_JJ security_NN ._.
--_: Such_JJ has_VBZ been_VBN the_DT patient_NN sufferance_NN of_IN these_DT Colonies_NNS ;_: and_CC such_JJ is_VBZ now_RB the_DT necessity_NN which_WDT constrains_VBZ them_PRP to_TO alter_VB their_PRP$ former_JJ Systems_NNP of_IN Government_NNP ._.
The_DT history_NN of_IN the_DT present_JJ King_NNP of_IN Great_NNP Britain_NNP is_VBZ a_DT history_NN of_IN repeated_JJ injuries_NNS and_CC usurpations_NNS ,_, all_DT having_VBG in_IN direct_JJ object_NN the_DT establishment_NN of_IN an_DT absolute_JJ Tyranny_NN over_IN these_DT States_NNP ._.
To_TO prove_VB this_DT ,_, let_VB Facts_NNS be_VB submitted_VBN to_TO a_DT candid_JJ world_NN ._.
He_PRP has_VBZ refused_VBN his_PRP$ Assent_NN to_TO Laws_NNS ,_, the_DT most_RBS wholesome_JJ and_CC necessary_JJ for_IN the_DT public_JJ good_NN ._.
He_PRP has_VBZ forbidden_VBN his_PRP$ Governors_NNS to_TO pass_VB Laws_NNS of_IN immediate_JJ and_CC pressing_JJ importance_NN ,_, unless_IN suspended_VBN in_IN their_PRP$ operation_NN till_IN his_PRP$ Assent_NN should_MD be_VB obtained_VBN ;_: and_CC when_WRB so_RB suspended_VBN ,_, he_PRP has_VBZ utterly_RB neglected_VBN to_TO attend_VB to_TO them_PRP ._.
He_PRP has_VBZ refused_VBN to_TO pass_VB other_JJ Laws_NNS for_IN the_DT accommodation_NN of_IN large_JJ districts_NNS of_IN people_NNS ,_, unless_IN those_DT people_NNS would_MD relinquish_VB the_DT right_NN of_IN Representation_NN in_IN the_DT Legislature_NNP ,_, a_DT right_RB inestimable_JJ to_TO them_PRP and_CC formidable_JJ to_TO tyrants_NNS only_RB ._.
He_PRP has_VBZ called_VBN together_RB legislative_JJ bodies_NNS at_IN places_NNS unusual_JJ ,_, uncomfortable_JJ ,_, and_CC distant_JJ from_IN the_DT depository_NN of_IN their_PRP$ public_JJ Records_NNP ,_, for_IN the_DT sole_JJ purpose_NN of_IN fatiguing_VBG them_PRP into_IN compliance_NN with_IN his_PRP$ measures_NNS ._.
He_PRP has_VBZ dissolved_VBN Representative_JJ Houses_NNS repeatedly_RB ,_, for_IN opposing_VBG with_IN manly_JJ firmness_NN his_PRP$ invasions_NNS on_IN the_DT rights_NNS of_IN the_DT people_NNS ._.
He_PRP has_VBZ refused_VBN for_IN a_DT long_JJ time_NN ,_, after_IN such_JJ dissolutions_NNS ,_, to_TO cause_VB others_NNS to_TO be_VB elected_VBN ;_: whereby_WRB the_DT Legislative_JJ powers_NNS ,_, incapable_JJ of_IN Annihilation_NNP ,_, have_VBP returned_VBN to_TO the_DT People_NNS at_IN large_JJ for_IN their_PRP$ exercise_NN ;_: the_DT State_NNP remaining_VBG in_IN the_DT mean_NN time_NN exposed_VBN to_TO all_PDT the_DT dangers_NNS of_IN invasion_NN from_IN without_IN ,_, and_CC convulsions_NNS within_IN ._.
He_PRP has_VBZ endeavoured_VBN to_TO prevent_VB the_DT population_NN of_IN these_DT States_NNS ;_: for_IN that_DT purpose_NN obstructing_VBG the_DT Laws_NNPS for_IN Naturalization_NNP of_IN Foreigners_NNS ;_: refusing_VBG to_TO pass_VB others_NNS to_TO encourage_VB their_PRP$ migrations_NNS hither_RB ,_, and_CC raising_VBG the_DT conditions_NNS of_IN new_JJ Appropriations_NNP of_IN Lands_NNPS ._.
He_PRP has_VBZ obstructed_VBN the_DT Administration_NNP of_IN Justice_NNP ,_, by_IN refusing_VBG his_PRP$ Assent_NN to_TO Laws_NNS for_IN establishing_VBG Judiciary_NNP powers_NNS ._.
He_PRP has_VBZ made_VBN Judges_NNP dependent_JJ on_IN his_PRP$ Will_NNP alone_RB ,_, for_IN the_DT tenure_NN of_IN their_PRP$ offices_NNS ,_, and_CC the_DT amount_NN and_CC payment_NN of_IN their_PRP$ salaries_NNS ._.
He_PRP has_VBZ erected_VBN a_DT multitude_NN of_IN New_JJ Offices_NNS ,_, and_CC sent_VBD hither_RB swarms_NNS of_IN Officers_NNS to_TO harrass_VB our_PRP$ people_NNS ,_, and_CC eat_VB out_RP their_PRP$ substance_NN ._.
He_PRP has_VBZ kept_VBN among_IN us_PRP ,_, in_IN times_NNS of_IN peace_NN ,_, Standing_VBG Armies_NNS without_IN the_DT Consent_NNP of_IN our_PRP$ legislatures_NNS ._.
He_PRP has_VBZ affected_VBN to_TO render_VB the_DT Military_NNP independent_JJ of_IN and_CC superior_JJ to_TO the_DT Civil_NNP power_NN ._.
He_PRP has_VBZ combined_VBN with_IN others_NNS to_TO subject_VB us_PRP to_TO a_DT jurisdiction_NN foreign_JJ to_TO our_PRP$ constitution_NN ,_, and_CC unacknowledged_JJ by_IN our_PRP$ laws_NNS ;_: giving_VBG his_PRP$ Assent_NN to_TO their_PRP$ Acts_NNS of_IN pretended_JJ Legislation_NN :_: For_IN Quartering_VBG large_JJ bodies_NNS of_IN armed_JJ troops_NNS among_IN us_PRP :_: For_IN protecting_VBG them_PRP ,_, by_IN a_DT mock_JJ Trial_NN ,_, from_IN punishment_NN for_IN any_DT Murders_NNS which_WDT they_PRP should_MD commit_VB on_IN the_DT Inhabitants_NNS of_IN these_DT States_NNS :_: For_IN cutting_VBG off_RP our_PRP$ Trade_NNP with_IN all_DT parts_NNS of_IN the_DT world_NN :_: For_IN imposing_VBG Taxes_NNS on_IN us_PRP without_IN our_PRP$ Consent_NNP :_: For_IN depriving_VBG us_PRP in_IN many_JJ cases_NNS ,_, of_IN the_DT benefits_NNS of_IN Trial_NN by_IN Jury_NN :_: For_IN transporting_VBG us_PRP beyond_IN Seas_NNS to_TO be_VB tried_VBN for_IN pretended_JJ offences_NNS For_IN abolishing_VBG the_DT free_JJ System_NN of_IN English_JJ Laws_NNS in_IN a_DT neighbouring_JJ Province_NNP ,_, establishing_VBG therein_RB an_DT Arbitrary_JJ government_NN ,_, and_CC enlarging_VBG its_PRP$ Boundaries_NNS so_RB as_IN to_TO render_VB it_PRP at_IN once_RB an_DT example_NN and_CC fit_NN instrument_NN for_IN introducing_VBG the_DT same_JJ absolute_JJ rule_NN into_IN these_DT Colonies_NNS :_: For_IN taking_VBG away_RP our_PRP$ Charters_NNS ,_, abolishing_VBG our_PRP$ most_RBS valuable_JJ Laws_NNS ,_, and_CC altering_VBG fundamentally_RB the_DT Forms_NNS of_IN our_PRP$ Governments_NNS :_: For_IN suspending_VBG our_PRP$ own_JJ Legislatures_NNS ,_, and_CC declaring_VBG themselves_PRP invested_VBD with_IN power_NN to_TO legislate_VB for_IN us_PRP in_IN all_DT cases_NNS whatsoever_RB ._.
He_PRP has_VBZ abdicated_VBN Government_NNP here_RB ,_, by_IN declaring_VBG us_PRP out_IN of_IN his_PRP$ Protection_NNP and_CC waging_VBG War_NNP against_IN us_PRP ._.
He_PRP has_VBZ plundered_VBN our_PRP$ seas_NNS ,_, ravaged_VBD our_PRP$ Coasts_NNP ,_, burnt_JJ our_PRP$ towns_NNS ,_, and_CC destroyed_VBD the_DT lives_NNS of_IN our_PRP$ people_NNS ._.
He_PRP is_VBZ at_IN this_DT time_NN transporting_VBG large_JJ Armies_NNS of_IN foreign_JJ Mercenaries_NNPS to_TO compleat_VB the_DT works_NNS of_IN death_NN ,_, desolation_NN and_CC tyranny_NN ,_, already_RB begun_VBN with_IN circumstances_NNS of_IN Cruelty_NNP &_CC perfidy_NN scarcely_RB paralleled_VBD in_IN the_DT most_RBS barbarous_JJ ages_NNS ,_, and_CC totally_RB unworthy_JJ the_DT Head_NNP of_IN a_DT civilized_JJ nation_NN ._.
He_PRP has_VBZ constrained_VBN our_PRP$ fellow_JJ Citizens_NNPS taken_VBN Captive_NNP on_IN the_DT high_JJ Seas_NNS to_TO bear_VB Arms_NNS against_IN their_PRP$ Country_NNP ,_, to_TO become_VB the_DT executioners_NNS of_IN their_PRP$ friends_NNS and_CC Brethren_NNS ,_, or_CC to_TO fall_VB themselves_PRP by_IN their_PRP$ Hands_NNS ._.
He_PRP has_VBZ excited_VBN domestic_JJ insurrections_NNS amongst_IN us_PRP ,_, and_CC has_VBZ endeavoured_VBN to_TO bring_VB on_IN the_DT inhabitants_NNS of_IN our_PRP$ frontiers_NNS ,_, the_DT merciless_JJ Indian_NNP Savages_NNP ,_, whose_WP$ known_JJ rule_NN of_IN warfare_NN ,_, is_VBZ an_DT undistinguished_JJ destruction_NN of_IN all_DT ages_NNS ,_, sexes_NNS and_CC conditions_NNS ._.
In_IN every_DT stage_NN of_IN these_DT Oppressions_NNS We_PRP have_VBP Petitioned_VBN for_IN Redress_NNP in_IN the_DT most_RBS humble_JJ terms_NNS :_: Our_PRP$ repeated_JJ Petitions_NNS have_VBP been_VBN answered_VBN only_RB by_IN repeated_VBN injury_NN ._.
A_DT Prince_NNP whose_WP$ character_NN is_VBZ thus_RB marked_VBN by_IN every_DT act_NN which_WDT may_MD define_VB a_DT Tyrant_NNP ,_, is_VBZ unfit_JJ to_TO be_VB the_DT ruler_NN of_IN a_DT free_JJ people_NNS ._.
Nor_CC have_VB We_PRP been_VBN wanting_VBG in_IN attentions_NNS to_TO our_PRP$ Brittish_JJ brethren_NNS ._.
We_PRP have_VBP warned_VBN them_PRP from_IN time_NN to_TO time_NN of_IN attempts_NNS by_IN their_PRP$ legislature_NN to_TO extend_VB an_DT unwarrantable_JJ jurisdiction_NN over_IN us_PRP ._.
We_PRP have_VBP reminded_VBN them_PRP of_IN the_DT circumstances_NNS of_IN our_PRP$ emigration_NN and_CC settlement_NN here_RB ._.
We_PRP have_VBP appealed_VBN to_TO their_PRP$ native_JJ justice_NN and_CC magnanimity_NN ,_, and_CC we_PRP have_VBP conjured_VBN them_PRP by_IN the_DT ties_NNS of_IN our_PRP$ common_JJ kindred_NN to_TO disavow_VB these_DT usurpations_NNS ,_, which_WDT ,_, would_MD inevitably_RB interrupt_VB our_PRP$ connections_NNS and_CC correspondence_NN ._.
They_PRP too_RB have_VBP been_VBN deaf_JJ to_TO the_DT voice_NN of_IN justice_NN and_CC of_IN consanguinity_NN ._.
We_PRP must_MD ,_, therefore_RB ,_, acquiesce_NN in_IN the_DT necessity_NN ,_, which_WDT denounces_VBZ our_PRP$ Separation_NN ,_, and_CC hold_VB them_PRP ,_, as_IN we_PRP hold_VBP the_DT rest_NN of_IN mankind_NN ,_, Enemies_NNPS in_IN War_NNP ,_, in_IN Peace_NNP Friends_NNPS ._.
We_PRP ,_, therefore_RB ,_, the_DT Representatives_NNP of_IN the_DT united_JJ States_NNP of_IN America_NNP ,_, in_IN General_NNP Congress_NNP ,_, Assembled_NNP ,_, appealing_VBG to_TO the_DT Supreme_NNP Judge_NNP of_IN the_DT world_NN for_IN the_DT rectitude_NN of_IN our_PRP$ intentions_NNS ,_, do_VB ,_, in_IN the_DT Name_NN ,_, and_CC by_IN Authority_NNP of_IN the_DT good_JJ People_NNS of_IN these_DT Colonies_NNS ,_, solemnly_RB publish_VB and_CC declare_VB ,_, That_IN these_DT United_NNP Colonies_NNPS are_VBP ,_, and_CC of_IN Right_NNP ought_MD to_TO be_VB Free_NNP and_CC Independent_NNP States_NNPS ;_: that_IN they_PRP are_VBP Absolved_JJ from_IN all_DT Allegiance_NNP to_TO the_DT British_NNP Crown_NNP ,_, and_CC that_IN all_DT political_JJ connection_NN between_IN them_PRP and_CC the_DT State_NNP of_IN Great_NNP Britain_NNP ,_, is_VBZ and_CC ought_MD to_TO be_VB totally_RB dissolved_VBN ;_: and_CC that_IN as_IN Free_NNP and_CC Independent_NNP States_NNPS ,_, they_PRP have_VBP full_JJ Power_NNP to_TO levy_VB War_NNP ,_, conclude_VBP Peace_NNP ,_, contract_NN Alliances_NNS ,_, establish_VB Commerce_NNP ,_, and_CC to_TO do_VB all_DT other_JJ Acts_NNS and_CC Things_NNS which_WDT Independent_NNP States_NNPS may_MD of_IN right_NN do_VBP ._.
And_CC for_IN the_DT support_NN of_IN this_DT Declaration_NNP ,_, with_IN a_DT firm_JJ reliance_NN on_IN the_DT protection_NN of_IN divine_JJ Providence_NNP ,_, we_PRP mutually_RB pledge_NN to_TO each_DT other_JJ our_PRP$ Lives_NNS ,_, our_PRP$ Fortunes_NNS and_CC our_PRP$ sacred_JJ Honor_NN ._.";
    }
}
